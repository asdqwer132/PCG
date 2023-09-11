using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TichuTurnManager : TurnManager
{
    [SerializeField] CompleteManager completeManager;
    [SerializeField] LanternManager turnLantern;
    [SerializeField] Submit submit;
    [SerializeField] Player startPlayer;//
    public bool IsAllPass() { return currentPlayer == startPlayer; }
    public void ResetAll() { turnLantern.ResetTurnLanter(); }
    public void SetStartTurn(int value)
    {
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), false);
        currentPlayer = TichuPlayerManager.GetAllPlayerWithTurn()[value];
        startPlayer = TichuPlayerManager.GetAllPlayerWithTurn()[GetCurrentPlayer()];
        turn = value;
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), true);
    }
    public override void NextTurn()
    {
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), false);
        if (turn > 0) turn--;
        else turn = TichuPlayerManager.MaxPlayer - 1;
        currentPlayer = TichuPlayerManager.GetAllPlayerWithTurn()[turn];
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), true);
        if (completeManager.CheckComplete(TichuPlayerManager.GetAllPlayerWithTurn()[GetCurrentPlayer()])) NextTurn();
    }
}
