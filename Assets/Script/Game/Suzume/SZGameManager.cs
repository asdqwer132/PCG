using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
public class SZGameManager : MonoBehaviour, IGameManagerInterface
{

    [Header("UI")]
    [SerializeField] HwaryoPopup hwaryoPopup;
    [SerializeField] SZPlayerOwnDisplay playerDisplay;
    [SerializeField] List<SZPlayerDisplay> otherDisplays = new List<SZPlayerDisplay>();
    [Header("Manager")]
    [SerializeField] DropManager dropManager;
    [SerializeField] SuzumeGenealogyChekcer genealogyChekcer;
    [SerializeField] SZCardDistributer cardDistributer;
    [SerializeField] SZTurnManager turnManager;
    [SerializeField] SZScoreManager scoreManager;
    [SerializeField] SZRoundManager roundManager;
    [SerializeField] SZCompleteManager completeManager;
    [SerializeField] List<CardInfo> playerCardPlusDropCard = new List<CardInfo>();//
    PhotonView PV;
    public static bool isGameOver = false;
    private void Start() { PV = GetComponent<PhotonView>(); }
    void RoundSet(bool isFirstRound)
    {
        playerDisplay.ResetPlayer();
        foreach (SZPlayerDisplay display in otherDisplays)
        {
            if (display.IsSetted())
            {
                display.ResetPlayer();
            }
        }
        PV.RPC("CardModify", RpcTarget.All, SerializeManager.Serialize(cardDistributer.ResetTotalCardList()));
        foreach (Player player in PlayerManager.GetAllPlayerWithTurn())
        {
            List<CardInfo> cardinfos = new List<CardInfo>();
            for (int i = 0; i < 5; i++)
            {
                cardinfos.Add(cardDistributer.Distribute());
            }
            player.TryGetCard(cardinfos);
        }
        PV.RPC("CardModify", RpcTarget.All, SerializeManager.Serialize(cardDistributer.TotalCardList));
        PV.RPC("RoundInitalize", RpcTarget.All, isFirstRound);
    }
    public void TryGameStart()
    {
        RoundSet(true);
    }
    public void TryRon()
    {
        SuzumePlayer player = (SuzumePlayer)PlayerManager.GetPlayerOwn();
        completeManager.DeclareRon(player);
        TryPass();
        playerCardPlusDropCard = PlayerManager.GetPlayerOwn().HandCards.ToList();
        playerCardPlusDropCard.Add(dropManager.CurrentDrop);
        playerCardPlusDropCard.Sort((x, y) => x.number.CompareTo(y.number));
        int score = genealogyChekcer.CheckGenealogy(playerCardPlusDropCard, dropManager.GetDoraCard());
        score += roundManager.IsStartPlayer(PlayerManager.GetPlayerOwn()) ? 2 : 0;
        scoreManager.CalculateRonScore(score, completeManager.GetRonTargetPlayers(), (SuzumePlayer)PlayerManager.GetPlayerOwn(), dropManager.GetDoraCard());
        PV.RPC("SetHwaryo", RpcTarget.All, PlayerManager.GetPlayerOwn().Index, score, "론!");
        if (!completeManager.IsWaiting())
        {
            RoundSet(false);
        }
    }
    public void TryTsumo()
    {
        SuzumePlayer playerOwn = (SuzumePlayer)PlayerManager.GetPlayerOwn();
        List<SuzumePlayer> others = new List<SuzumePlayer>();
        foreach (Player player in PlayerManager.GetAllPlayerWithTurn())
        {
            if (player.Index != playerOwn.Index)
            {
                SuzumePlayer suzumePlayer = (SuzumePlayer)player;
                others.Add(suzumePlayer);
            }
        }
        int score = genealogyChekcer.CheckGenealogy(PlayerManager.GetPlayerOwn().HandCards, dropManager.GetDoraCard());
        score += roundManager.IsStartPlayer(PlayerManager.GetPlayerOwn()) ? 2 : 0;
        scoreManager.CalculateTsumoScore(score, playerOwn, others);
        PV.RPC("SetHwaryo", RpcTarget.All, PlayerManager.GetPlayerOwn().Index, score, "쯔모!");
        RoundSet(false);
    }
    public void TryDrop()
    {
        if (!dropManager.IsDropSetted()) return;
        if (!turnManager.IsMyTurn()) return;
        playerDisplay.SetTsumoBtn(false);
        playerDisplay.GetPlayer.TryDropCard(dropManager.GetDropCardInfo());
        PV.RPC("DropCard", RpcTarget.All, SerializeManager.Serialize(dropManager.GetDropCardInfo()), PlayerManager.GetPlayerOwn().Index);
        dropManager.HideDropCard();
        PV.RPC("NextTurn", RpcTarget.All);
    }
    public void TryDraw()
    {
        if (!turnManager.IsMyTurn()) return;
        if (completeManager.IsWaiting()) return; 
        if (cardDistributer.CardAllUsed())
        {
            //유국
            RoundSet(false);
            return;
        } 
        playerDisplay.SetDrawBtn(false);
        dropManager.SetDropArea(true);
        CardInfo newCard = cardDistributer.Distribute();
        PlayerManager.GetPlayerOwn().TryGetCard(new List<CardInfo>() { newCard });
        PV.RPC("CardModify", RpcTarget.All, SerializeManager.Serialize(cardDistributer.TotalCardList));
        dropManager.SetCard(newCard);
        playerDisplay.SetCurrentScore(0);
        int score = genealogyChekcer.CheckGenealogy(PlayerManager.GetPlayerOwn().HandCards, dropManager.GetDoraCard());
        if (score >= 5)
        {
            //쯔모
            score += roundManager.IsStartPlayer(PlayerManager.GetPlayerOwn()) ? 2 : 0;
            playerDisplay.SetCurrentScore(score);
            playerDisplay.SetTsumoBtn(true);
        }
    }
    public void TryPass()
    {
        PV.RPC("Pass", RpcTarget.All, PlayerManager.GetPlayerOwn().Index);
        playerDisplay.SetRonBtn(false);
    }
    [PunRPC]
    void SetHwaryo(int index,int score, string howToWin)
    {
        SuzumePlayer player = (SuzumePlayer)PlayerManager.GetPlayerWithTurn(index);
        CardInfo cardInfo = howToWin == "론!" ? new CardInfo(dropManager.CurrentDrop.cardType, dropManager.CurrentDrop.number) : new CardInfo("", 0); 
        hwaryoPopup.Setup(score, player.HandCards, howToWin, cardInfo);
    }
    [PunRPC]
    void Pass(int index)
    {
        completeManager.SetWaiting(false, PlayerManager.GetPlayerWithTurn(index), null);
    }
    [PunRPC]
    void RoundInitalize(bool isFirstRound)
    {
        if (!isFirstRound)
        {
            hwaryoPopup.NextSlot();
            roundManager.NextTurn();
            turnManager.SetStartTurn(roundManager.GetCurrentPlayer());
            if (roundManager.IsRoundOver())
            {
                roundManager.RoundOver();
                if (roundManager.IsGameOver())
                {
                    //게임 종료
                    isGameOver = true;
                }
            }
        }
        else//첫라운드
        {
            isGameOver = false;
            roundManager.SetStartTurn(0);
            turnManager.SetStartTurn(0);
        }
        dropManager.SetDropArea(false);
        dropManager.SetDora(cardDistributer.Distribute());
        cardDistributer.SetLeftCardCount();
        playerDisplay.ResetDeclareBtn();
        playerDisplay.SetDrawBtn(turnManager.IsMyTurn());
        playerDisplay.Setup(PlayerManager.GetPlayerOwn().HandCards);
        playerDisplay.SetFirstMark(roundManager.GetCurrentPlayer());
        playerDisplay.SetScore();
        dropManager.ResetDrop();
        completeManager.ResetRonPlayer();
        foreach (SZPlayerDisplay display in otherDisplays)
        {
            if (display.IsSetted())
            {
                display.SetScore();
                display.SetDropCardList();
                display.SetFirstMark(roundManager.GetCurrentPlayer());
            }
        }
    }
    [PunRPC]
    void DropCard(string cardInfo, int index)
    {
         CardInfo cardList = SerializeManager.Deserialize< CardInfo>(cardInfo);
         dropManager.SetDropCard(cardList, index);
    }
    [PunRPC]
    void CardModify(string cardInfo)
    {
        List<CardInfo> cardList = SerializeManager.Deserialize<List<CardInfo>>(cardInfo);
        cardDistributer.TotalCardList = cardList;
        cardDistributer.SetLeftCardCount();
    }
    [PunRPC]
    void SetRon(int index, int targetIndex)
    {
        completeManager.SetWaiting(true, PlayerManager.GetPlayerWithTurn(index), (SuzumePlayer)PlayerManager.GetPlayerWithTurn(targetIndex));
    }
    [PunRPC]
    void NextTurn()
    {
        playerCardPlusDropCard = PlayerManager.GetPlayerOwn().HandCards.ToList();
        playerCardPlusDropCard.Add(dropManager.CurrentDrop);
        playerCardPlusDropCard.Sort((x, y) => x.number.CompareTo(y.number));
        int score = genealogyChekcer.CheckGenealogy(playerCardPlusDropCard, dropManager.GetDoraCard());
        playerDisplay.SetCurrentScore(0);
        SuzumePlayer player = (SuzumePlayer)PlayerManager.GetPlayerOwn();
        if (score >= 5 && !dropManager.IsOwnDrop() && !dropManager.IsDroped(player.GetDropCard) && completeManager.IsNotDroped(dropManager.CurrentDrop, player))
        {
            //론
            score += roundManager.IsStartPlayer(PlayerManager.GetPlayerOwn()) ? 2 : 0;
            playerDisplay.SetCurrentScore(score);
            playerDisplay.SetRonBtn(true);
            PV.RPC("SetRon", RpcTarget.All, PlayerManager.GetPlayerOwn().Index, turnManager.GetCurrentPlayer());
        }
        turnManager.NextTurn();
        foreach (SZPlayerDisplay display in otherDisplays)
        {
            if(display.gameObject.activeSelf) display.SetDropCardList();
        }
        if (turnManager.IsMyTurn())
        {
            playerDisplay.SetDrawBtn(true);
        }
    }
}
