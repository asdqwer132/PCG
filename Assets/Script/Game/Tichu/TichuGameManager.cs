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
    [SerializeField] CardDistributer cardDistributer;
    [SerializeField] TichuTurnManager turnManager;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] SubmitManager submitManager;
    [SerializeField] CompleteManager completeManager;
    [SerializeField] RoundReadyManager roundManager;
    [SerializeField] TichuDeclareManager tichuManager;
    [SerializeField] PannelManager PannelManager;
    [SerializeField] SwapManager swapManager;
    [SerializeField] CallPannel callPannel;
    [SerializeField] LanternManager lanternManager;
    [SerializeField] TichuVictoryManager victoryManager;
    [SerializeField] TichuModeManager modeManager;
    PhotonView PV;
    private void Start() { PV = GetComponent<PhotonView>(); }
    #region Round
    public void GameStart()
    {
        PV.RPC("GameIntailize", RpcTarget.All, SerializeManager.Serialize(cardDistributer.ResetTotalCardList()));
        cardDistributer.FirstDistribute(TichuPlayerManager.GetAllPlayerWithTurn());
        PV.RPC("AfterDistribute", RpcTarget.All, SerializeManager.Serialize(cardDistributer.TotalCardList), false);
    }
    void RoundStart()
    {
        foreach (CardInfo cardInfo in TichuPlayerManager.GetPlayerOwn().HandCards)
        {
            if (cardInfo.number == 1)
            {
                PV.RPC("SetStartTurn", RpcTarget.All, TichuPlayerManager.GetPlayerOwn().Index);
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

        TichuPlayer player = (TichuPlayer)TichuPlayerManager.GetPlayerOwn();
        if (player.Tichu == Tichu.smallTichu) { PV.RPC("DeclareTichu", RpcTarget.All, TichuPlayerManager.GetPlayerOwn().Index); return; }
        if (tichuManager.IsAllDecideTichu())//Ƽ�򼱾�Ϸ�
        {
            PV.RPC("AfterAllTichuAction", RpcTarget.All);
            cardDistributer.SecondDistribute(TichuPlayerManager.GetAllPlayerWithTurn());
            PV.RPC("AfterDistribute", RpcTarget.All, SerializeManager.Serialize(cardDistributer.TotalCardList), true);
        }
    }
    public void TryCheckSwap()
    {
        PV.RPC("AfterDistribute", RpcTarget.All, SerializeManager.Serialize(cardDistributer.TotalCardList), true);
        if (swapManager.IsAllSwap())//���� ���� ����
        {
            PV.RPC("AfterAllSwapAction", RpcTarget.All); RoundStart();
        }
    }
    #endregion
    #region Game
    public void TrySendDragon(int index, int score) { PV.RPC("SendDragon", RpcTarget.All, index, score); }
    public void TrySubmit(bool isBirdStart)
    {
        if (turnManager.IsMyTurn() || GenealogyChekcer.IsBomb(cardChecker.SelectedCards))
        {
            if (!cardChecker.IsSelected()) return;//ī�� ���õ� üĿ
            if (callPannel.IsSetted() && !isBirdStart)//�ݸ� üĿ
            {
                if (callPannel.CheckCall(TichuPlayerManager.GetPlayerOwn().HandCards, submitManager.GetGenealogy()))//�� �ɷȴµ� �� �� �ִµ� �н��Ϸ���
                {
                    if (!callPannel.CheckCall(cardChecker.SelectedCards, submitManager.GetGenealogy()))//�� �ɷȴµ� ���� ������
                    {
                        callPannel.SetMessage(WanringType.submit);
                        return;
                    }
                    PV.RPC("Call", RpcTarget.All, -99);
                }
            }
            //±±��
            if (AnimalChecker.CheckAnimal(Animal.bird, cardChecker.SelectedCards))
            {
                if (callPannel.IsSetted()) { PV.RPC("Call", RpcTarget.All, callPannel.GetCall()); }
                else
                {
                    callPannel.Toggle();
                    return;
                }
            }
            //�����
            TichuPlayerManager.GetPlayerOwn().TryUseCard(cardChecker.SelectedCards);
            PV.RPC("Submit", RpcTarget.All, SerializeManager.Serialize(cardChecker.SelectedCards), TichuPlayerManager.GetPlayerOwn().Index);
            //������ ���� Ȯ��
            if (TichuPlayerManager.GetPlayerOwn().HandCards.Count == 0)
            {
                PV.RPC("Complete", RpcTarget.All, TichuPlayerManager.GetPlayerOwn().Index);

                if (victoryManager.IsEnd()) { return; }
                if (completeManager.IsRoundOver())//��� ���� ����
                {
                    GameStart();//���� �����
                    return;
                }
            }
            //�����
            if (AnimalChecker.CheckAnimal(Animal.dog, cardChecker.SelectedCards)) { PV.RPC("Dog", RpcTarget.All, TichuPlayerManager.GetPlayerOwn().Index); }
            //�ļ�ó��
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
            if (submitManager.IsFirst()) return; //Ʈ�� ���� �÷��̾� üĿ
            if (callPannel.IsSetted())//�ݸ� üĿ
            {
                if (callPannel.CheckCall(TichuPlayerManager.GetPlayerOwn().HandCards, submitManager.GetGenealogy()))//�� �ɷȴµ� �� �� �ִµ� �н��Ϸ���
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
    #region Pun�Լ�
    [PunRPC]
    void SetMode()
    {
        modeManager.SetMode();
    }
    [PunRPC]
    void SendDragon(int index, int score)//Ƽ���ư�� �Ҵ� (��� ����Ƽ�� ���� ��)
    {
        scoreManager.AddTemporaryScore(score, index); 
        PannelManager.ResetPannel(false);
        if (TichuPlayerManager.GetPlayerOwn().HandCards.Count > 0) PannelManager.ActiveSubmitArea(true);
    }
    [PunRPC]
    void Call(int callRank)//Ƽ���ư�� �Ҵ� (��� ����Ƽ�� ���� ��)
    {
        callPannel.SetCall(callRank);
    }
    [PunRPC]
    void Dog(int index)
    {
        if (DistanceCalculater.GetDistanceWithRoot(index, turnManager.GetCurrentPlayer()) == 1) turnManager.NextTurn();
        submitManager.ResetAll();
    }
    [PunRPC]
    void AfterAllTichuAction()//Ƽ���ư�� �Ҵ� (��� ����Ƽ�� ���� ��)
    {
        PannelManager.ActiveSendArea(true);
        TichuPlayer player = (TichuPlayer)TichuPlayerManager.GetPlayerOwn();
        if (player.Tichu != Tichu.largeTichu) PannelManager.ActiveSmallTichuArea(true);
        tichuManager.SetBackground(Tichu.noTichu);
        if (tichuManager.HasTichu(Tichu.largeTichu))
        {
            tichuManager.SetBackground(Tichu.largeTichu);
            SoundEffecter.Instance.PlayEffect("largeTichu");
            foreach(CardDisplayer cardDisplayer in cardDisplayers)
            {
                cardDisplayer.SetTichu();
            }
        }//��� Ƽ�� ���� ��
        swapManager.TogglePannel(true);
    }
    [PunRPC]
    void AfterAllSwapAction()//���ҹ�ư�� �Ҵ� (��� ���� ��)
    {
        PannelManager.ActiveSubmitArea(true);
        TichuPlayer playerOwn = (TichuPlayer)TichuPlayerManager.GetPlayerOwn();
        if (playerOwn.Tichu == Tichu.noTichu) PannelManager.ActiveSmallTichuArea(true);
        swapManager.TogglePannel(false);
        swapManager.OpenPopup();

        foreach (TichuPlayer player in TichuPlayerManager.GetAllPlayerWithTurn())
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
    void StartRound()//���ҹ�ư�� �Ҵ� (��� ���� ��)
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
    void DeclareTichu(int playerIndex)//���ҹ�ư�� �Ҵ�
    {
        TichuPlayer player = (TichuPlayer)TichuPlayerManager.GetAllPlayerWithTurn()[playerIndex];
        Tichu tichu = player.Tichu;
        if(tichu != Tichu.noTichu)
        {
            tichuManager.SetBackground(tichu);
            SoundEffecter.Instance.PlayEffect(tichu.ToString()); ;
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
        List<CardInfo> cardInfos = SerializeManager.Deserialize<List<CardInfo>>(selectedCard);
        cardDistributer.TotalCardList = cardInfos;
        PannelManager.ActiveLargeTichuArea(true);
    }
    [PunRPC]
    void SetStartTurn(int index)
    {
        TichuPlayer player = (TichuPlayer)TichuPlayerManager.GetAllPlayerWithTurn()[index];
        turnManager.SetStartTurn(player.Index);
        submit.SetTurn(turnManager.IsMyTurn());
    }
    [PunRPC]
    void Submit(string selectedCard, int index)
    {
        lanternManager.ResetPassLanter();
        List<CardInfo> cardInfos = SerializeManager.Deserialize<List<CardInfo>>(selectedCard);
        Genealogy genealogy = GenealogyChekcer.CheckGenealogy(cardInfos);

        //����
        if(callPannel.GetCall() != -99 && cardInfos.Find(x => x.number == 1) != null) SoundEffecter.Instance.PlayeEffectByResources(callPannel.GetCall() + "c");
        else SoundEffecter.Instance.PlayeEffectByResources(genealogy.rank + genealogy.genealogyType.ToString());
        //��
        if (GenealogyChekcer.IsBomb(cardInfos))
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
        SoundEffecter.Instance.PlayEffect("pass");
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
        TichuPlayer player = (TichuPlayer)TichuPlayerManager.GetAllPlayerWithTurn()[playerIndex];
        completeManager.Complete(player);
        //turnManager.NextTurn();
        turnManager.SetStartTurn(turnManager.GetCurrentPlayer());
        if (completeManager.IsRoundOver())//��� ���� ����
        {
            scoreManager.AddTemporaryScore(submitManager.GetScore(), turnManager.GetCurrentPlayer());
            scoreManager.FinishRound();//���� ���
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
        submit.SetBtn(submitManager.CheckCanSubmit(GenealogyChekcer.CheckGenealogy(cardChecker.SelectedCards)));
        if (GenealogyChekcer.IsBomb(cardChecker.SelectedCards)) submit.ForceSetBtn();
        foreach (CardDisplayer cardDisplayer in cardDisplayers) { cardDisplayer.SetCard(); }
    }
    #endregion
}
