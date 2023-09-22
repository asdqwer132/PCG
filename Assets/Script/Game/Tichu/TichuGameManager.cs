using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TichuGameManager : MonoBehaviour, IGameManagerInterface
{
    [Header("UI")]
    [SerializeField] List<CardDisplayer> cardDisplayers = new List<CardDisplayer>();
    [SerializeField] CardChecker cardChecker;
    [SerializeField] Submit submit;
    [Header("Manager")]
    [SerializeField] Popup dogAnimation;
    [SerializeField] TichuCardDistributer cardDistributer;
    [SerializeField] TichuTurnManager turnManager;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] SubmitManager submitManager;
    [SerializeField] CompleteManager completeManager;
    [SerializeField] RoundReadyManager roundManager;
    [SerializeField] TichuDeclareManager tichuManager;
    [SerializeField] PannelManager PannelManager;
    [SerializeField] SwapManager swapManager;
    [SerializeField] CallPannel callPannel;
    [SerializeField] TichuLanternManager lanternManager;
    [SerializeField] TichuVictoryManager victoryManager;
    [SerializeField] TichuModeManager modeManager;
    PhotonView PV;
    bool isBirdFlag = false;
    private void Start() { PV = GetComponent<PhotonView>(); }
    #region Round
    public void TryGameStart()
    {
        PV.RPC("GameIntailize", RpcTarget.All, SerializeManager.Serialize(cardDistributer.ResetTotalCardList()));
        cardDistributer.FirstDistribute(PlayerManager.GetAllPlayerWithTurn());
        PV.RPC("AfterDistribute", RpcTarget.All, SerializeManager.Serialize(cardDistributer.TotalCardList), false);
    }
    void RoundStart()
    {
        foreach (CardInfo cardInfo in PlayerManager.GetPlayerOwn().HandCards)
        {
            if (cardInfo.number == 1)
            {
                PV.RPC("SetStartTurn", RpcTarget.All, PlayerManager.GetPlayerOwn().Index);
            }
        }
    }
    #endregion
    #region RoundSetting
    public void TrySetMode()
    {
        PV.RPC("SetMode", RpcTarget.All);
    }
    public void TryCheckTIchu()
    {

        TichuPlayer player = (TichuPlayer)PlayerManager.GetPlayerOwn();
        if (player.Tichu == Tichu.smallTichu) { PV.RPC("DeclareTichu", RpcTarget.All, PlayerManager.GetPlayerOwn().Index); return; }
        if (tichuManager.IsAllDecideTichu())//티츄선언완료
        {
            PV.RPC("AfterAllTichuAction", RpcTarget.All);
            cardDistributer.SecondDistribute(PlayerManager.GetAllPlayerWithTurn());
            PV.RPC("AfterDistribute", RpcTarget.All, SerializeManager.Serialize(cardDistributer.TotalCardList), true);
        }
    }
    public void TryCheckSwap()
    {
        PV.RPC("AfterDistribute", RpcTarget.All, SerializeManager.Serialize(cardDistributer.TotalCardList), true);
        if (swapManager.IsAllSwap())//스왑 완전 종료
        {
            PV.RPC("AfterAllSwapAction", RpcTarget.All); RoundStart();
        }
    }
    #endregion
    #region Game
    public void TrySendDragon(int index, int score) { PV.RPC("SendDragon", RpcTarget.All, index, score); }
    public void TrySubmit()
    {
        if (turnManager.IsMyTurn() || (GenealogyChekcer.Instance.IsBomb(cardChecker.SelectedCards) && !submitManager.IsFirst()))
        {
            if (!cardChecker.IsSelected()) return;//카드 선택됨 체커
            if (callPannel.IsSetted() && !isBirdFlag)//콜링 체커
            {
                if (callPannel.CheckCall(PlayerManager.GetPlayerOwn().HandCards, submitManager.GetGenealogy()))//콜을 낼 수 있음
                {
                    if (!callPannel.CheckCall(cardChecker.SelectedCards, submitManager.GetGenealogy()))//콜 걸렸는데 딴거 내려함
                    {
                        callPannel.SetMessage(WanringType.submit);
                        return;
                    }
                    PV.RPC("Call", RpcTarget.All, -99);
                }
            }
            //짹짹이
            if (AnimalChecker.CheckAnimal(Animal.bird, cardChecker.SelectedCards))
            {
                if (callPannel.IsSetted()) { PV.RPC("Call", RpcTarget.All, callPannel.GetCall()); isBirdFlag = false; }
                else
                {
                    callPannel.Toggle();
                    isBirdFlag = true;
                    return;
                }
            }
            //제출부
            PlayerManager.GetPlayerOwn().TryUseCard(cardChecker.SelectedCards);
            PV.RPC("Submit", RpcTarget.All, SerializeManager.Serialize(cardChecker.SelectedCards), PlayerManager.GetPlayerOwn().Index);
            //제출후 완주 확인
            if (PlayerManager.GetPlayerOwn().HandCards.Count == 0)
            {
                PV.RPC("Complete", RpcTarget.All, PlayerManager.GetPlayerOwn().Index);

                if (victoryManager.IsEnd()) { return; }
                if (completeManager.IsRoundOver())//모든 라운드 종료
                {
                    TryGameStart();//다음 라운드로
                    return;
                }
            }
            //댕댕이
            if (AnimalChecker.CheckAnimal(Animal.dog, cardChecker.SelectedCards)) { PV.RPC("Dog", RpcTarget.All, PlayerManager.GetPlayerOwn().Index); }
            //후속처리
            PannelManager.ActiveSmallTichuArea(false);
            cardChecker.HideCard();
            cardChecker.ResetCards();
            cardChecker.CardSetup();
            PV.RPC("SetDisplay", RpcTarget.All);
        }
    }
    public void TryPass()
    {
        if (turnManager.IsMyTurn())
        {
            if (submitManager.IsFirst()) return; //트릭 시작 플레이어 체커
            if (callPannel.IsSetted())//콜링 체커
            {
                if (callPannel.CheckCall(PlayerManager.GetPlayerOwn().HandCards, submitManager.GetGenealogy()))//콜 걸렸는데 낼 수 있는데 패스하려함
                {
                    callPannel.SetMessage(WanringType.pass);
                    return;
                }
            }
            PV.RPC("Pass", RpcTarget.All);
            PV.RPC("SetDisplay", RpcTarget.All);
            cardChecker.CardSetup();
        }
    }
    #endregion
    #region Pun함수
    [PunRPC]
    void SetMode()
    {
        modeManager.SetMode();
    }
    [PunRPC]
    void SendDragon(int index, int score)//티츄버튼에 할당 (모든 라지티츄 선언 후)
    {
        scoreManager.AddTemporaryScore(score, index); 
        PannelManager.ResetPannel(false);
        if (PlayerManager.GetPlayerOwn().HandCards.Count > 0) PannelManager.ActiveSubmitArea(true);
    }
    [PunRPC]
    void Call(int callRank)//티츄버튼에 할당 (모든 라지티츄 선언 후)
    {
        callPannel.SetCall(callRank);
    }
    [PunRPC]
    void Dog(int index)
    {
        dogAnimation.Toggle();
        if (DistanceCalculater.GetDistanceWithRoot(index, turnManager.GetCurrentPlayer()) == (int)Direction.right) turnManager.NextTurn();
        submitManager.ResetAll();
    }
    [PunRPC]
    void AfterAllTichuAction()//티츄버튼에 할당 (모든 라지티츄 선언 후)
    {
        PannelManager.ActiveSendArea(true);
        TichuPlayer player = (TichuPlayer)PlayerManager.GetPlayerOwn();
        if (player.Tichu != Tichu.largeTichu) PannelManager.ActiveSmallTichuArea(true);
        tichuManager.SetBackground(Tichu.noTichu);
        if (tichuManager.HasTichu(Tichu.largeTichu))
        {
            tichuManager.SetBackground(Tichu.largeTichu);
            SoundEffecter.Instance.PlayEffect(SoundType.game,"largeTichu");
            foreach(CardDisplayer cardDisplayer in cardDisplayers)
            {
                cardDisplayer.SetTichu();
            }
        }//모든 티츄 선언 끝
        swapManager.TogglePannel(true);
    }
    [PunRPC]
    void AfterAllSwapAction()//스왑버튼에 할당 (모든 스왑 후)
    {
        PannelManager.ActiveSubmitArea(true);
        TichuPlayer playerOwn = (TichuPlayer)PlayerManager.GetPlayerOwn();
        if (playerOwn.Tichu == Tichu.noTichu) PannelManager.ActiveSmallTichuArea(true);
        swapManager.TogglePannel(false);
        swapManager.OpenPopup();

        foreach (TichuPlayer player in PlayerManager.GetAllPlayerWithTurn())
        {
            foreach (CardReciever cardReciever in player.RecieveCard)
            {
                player.HandCards.Add(cardReciever.cardInfo);
            }
            player.HandCards.Sort((x, y) => x.cardType.CompareTo(y.cardType));
            player.HandCards.Sort((x, y) => x.number.CompareTo(y.number));
        }
        cardChecker.CardSetup();
        foreach (CardDisplayer cardDisplayer in cardDisplayers)
        {
            cardDisplayer.SetSendRecieve(false);
            cardDisplayer.SetCard();
        }
        RoundStart();
    }
    [PunRPC]
    void StartRound()//스왑버튼에 할당 (모든 스왑 후)
    {
        foreach (CardDisplayer cardDisplayer in cardDisplayers)
        {
            cardDisplayer.SetSendRecieve(false);
            cardDisplayer.SetCard();
        }
        RoundStart();
    }
    [PunRPC]
    void AfterDistribute(string selectedCard, bool isSwap)
    {
        List<CardInfo> cardInfos = SerializeManager.Deserialize<List<CardInfo>>(selectedCard);
        cardDistributer.TotalCardList = cardInfos;
        foreach (CardDisplayer cardDisplayer in cardDisplayers)
        {
            cardDisplayer.SetSendRecieve(isSwap);
            cardDisplayer.SetCard();
        }
        cardChecker.ResetCards();
        cardChecker.CardSetup();
    }
    [PunRPC]
    void DeclareTichu(int playerIndex)//스왑버튼에 할당
    {
        TichuPlayer player = (TichuPlayer)PlayerManager.GetAllPlayerWithTurn()[playerIndex];
        Tichu tichu = player.Tichu;
        if(tichu != Tichu.noTichu)
        {
            tichuManager.SetBackground(tichu);
            SoundEffecter.Instance.PlayEffect(SoundType.game, tichu.ToString()); ;
            int distance = DistanceCalculater.GetDistance(playerIndex);
            if (distance == 0) PannelManager.ActiveTichuLogo(tichu);
            else foreach(CardDisplayer cardDisplayer in cardDisplayers) { cardDisplayer.SetTichu(); }
        }
    }
    [PunRPC]
    void GameIntailize(string selectedCard)
    {
        cardChecker.ResetCards();
        turnManager.ResetAll();
        submitManager.ResetAll();
        completeManager.ResetAll();
        tichuManager.ResetAll();
        callPannel.ResetCall();
        PannelManager.ResetPannel(true);
        lanternManager.ResetPassLanter();
        PlayMusicOperator.Instance.ResetBgm();
        List<CardInfo> cardInfos = SerializeManager.Deserialize<List<CardInfo>>(selectedCard);
        cardDistributer.TotalCardList = cardInfos;
        PannelManager.ActiveLargeTichuArea(true);
    }
    [PunRPC]
    void SetStartTurn(int index)
    {
        TichuPlayer player = (TichuPlayer)PlayerManager.GetAllPlayerWithTurn()[index];
        turnManager.SetStartTurn(player.Index);
        submit.SetTurn(turnManager.IsMyTurn());
    }
    [PunRPC]
    void Submit(string selectedCard, int index)
    {
        lanternManager.ResetPassLanter();
        List<CardInfo> cardInfos = SerializeManager.Deserialize<List<CardInfo>>(selectedCard);
        Genealogy genealogy = GenealogyChekcer.Instance.CheckGenealogy(cardInfos);

        //사운드
        if(callPannel.GetCall() != -99 && cardInfos.Find(x => x.number == 1) != null) SoundEffecter.Instance.PlayeEffectByResources(callPannel.GetCall() + "c");
        else SoundEffecter.Instance.PlayeEffectByResources(genealogy.rank + genealogy.genealogyType.ToString());
        //뿜
        if (GenealogyChekcer.Instance.IsBomb(cardInfos))
        {
            turnManager.SetStartTurn(index);
        }
        else
        {
            turnManager.SetStartTurn(turnManager.GetCurrentPlayer());
        }
        turnManager.NextTurn();
        submitManager.Submit(cardInfos,index);
    }
    [PunRPC]
    void Pass()
    {
        SoundEffecter.Instance.PlayEffect(SoundType.game, "pass");
        lanternManager.SetPassLantern(DistanceCalculater.GetDistance(turnManager.GetCurrentPlayer()), true);
        turnManager.NextTurn();
        if (turnManager.IsAllPass())
        {
            lanternManager.ResetPassLanter();
            if (submitManager.IsDragon()) PannelManager.SetupDragon(submitManager.GetScore());
            else scoreManager.AddTemporaryScore(submitManager.GetScore(), submitManager.GetSubmitPlayer().Index);
            submitManager.ResetAll();
        }
    }
    [PunRPC]
    void Complete(int playerIndex)
    {
        TichuPlayer player = (TichuPlayer)PlayerManager.GetAllPlayerWithTurn()[playerIndex];
        completeManager.Complete(player);
        //turnManager.NextTurn();
        turnManager.SetStartTurn(turnManager.GetCurrentPlayer());
        if (completeManager.IsRoundOver())//모든 라운드 종료
        {
            scoreManager.AddTemporaryScore(submitManager.GetScore(), turnManager.GetCurrentPlayer());
            scoreManager.FinishRound();//점수 계산
            if (victoryManager.IsEnd())
            {
                victoryManager.GameOver();
            }
        }
    }
    [PunRPC]
    void SetDisplay()
    {
        submit.SetTurn(turnManager.IsMyTurn());
        submit.SetBtn(submitManager.CheckCanSubmit(GenealogyChekcer.Instance.CheckGenealogy(cardChecker.SelectedCards)));
        if (GenealogyChekcer.Instance.IsBomb(cardChecker.SelectedCards)) submit.ForceSetBtn();
        foreach (CardDisplayer cardDisplayer in cardDisplayers) { cardDisplayer.SetCard(); }
    }
    #endregion
}
