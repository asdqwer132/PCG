using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NDGameManager : MonoBehaviour, IGameManagerInterface
{
    [Header("UI")]
    [SerializeField] NDWinnerPopup winnerPopup;
    [SerializeField] NDVictoryPopup victoryPopup;
    [SerializeField] TmpAlpha waringMessage;
    [SerializeField] CardListDisplayer playerCard;
    [SerializeField] NDEnemyDisplayer enemyCard;
    [SerializeField] NDRoundResultPopup roundResultPopup;
    [Header("Manager")]
    [SerializeField] NDBattleManager battleManager;
    [SerializeField] NDCardDistributer cardDistributer;
    [SerializeField] NDSubmitManager submitManager;
    [SerializeField] NDTurnManager turnManager;
    PhotonView PV;
    private void Start() { PV = GetComponent<PhotonView>(); }
    public void TryGameStart()
    {
        cardDistributer.Distribute(PlayerManager.GetAllPlayerWithTurn());
        PV.RPC("SetDisplayer", RpcTarget.All);
    }
    public void TrySubmit()
    {
        if (!turnManager.IsMyTurn())
        {
            waringMessage.FadeOut(0);
            return;
        }
        if (!submitManager.CanSubmit())
        {
            waringMessage.FadeOut(1);
            return;
        }
        PV.RPC("Submit", RpcTarget.All, submitManager.GetSubmitRank(), PlayerManager.GetPlayerOwn().Index);
    }
    [PunRPC]
    void SetDisplayer()
    {
        playerCard.SetCard(PlayerManager.GetPlayerOwn().HandCards);
        enemyCard.SetLanern(PlayerManager.GetPlayerWithDirection((int)Direction.other).HandCards);
        turnManager.SetStartTurn(Team.Blue.ToString());//게임 시작 플레이어
    }
    [PunRPC]
    void Submit(int rank, int index)
    {
        //제출 카드 확인
        Team team = PlayerManager.GetPlayerWithTurn(index).Team;
        bool isMine = PlayerManager.GetPlayerWithTurn(index) == PlayerManager.GetPlayerOwn();
        submitManager.AddSubmit(rank, isMine, team);
        turnManager.NextTurn();
        if (!isMine)//상대편 카드 정리
        {
            enemyCard.SetSubmitLantern(rank, team.ToString());
        }
        if (submitManager.IsEnd())//라운드 종료 후 정리
        {
            Winner winner = battleManager.WhoIsWinner(submitManager.GetSubmits()[(int)Direction.own], submitManager.GetSubmits()[(int)Direction.other]);
            roundResultPopup.SetRound(submitManager.GetSubmits(), winner);
            Winner FinalWInner = roundResultPopup.CalculateWinner();
            winnerPopup.OpenPopup(winner);
            if (FinalWInner != Winner.none)
            {
                victoryPopup.GameOver(FinalWInner, roundResultPopup.GetAllRound());
                return;
            }

            if(winner != Winner.Draw) turnManager.SetStartTurn(winner.ToString());
            enemyCard.ResetSubmitLantern();
            submitManager.Submit();
            submitManager.ResetSubmit();
        }
    }
}
