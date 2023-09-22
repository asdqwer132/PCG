using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TichuTurnManager : RotationManager
{
    [SerializeField] CompleteManager completeManager;
    [SerializeField] TichuLanternManager turnLantern;
    [SerializeField] Submit submit;
    public void ResetAll() { turnLantern.ResetTurnLanter(); }
    public override void SetStartTurn(int value)
    {
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), false);
        base.SetStartTurn(value);
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), true);
    }
    public override void NextTurn()
    {
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), false);
        base.NextTurn();
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), true);
        if (completeManager.CheckComplete(PlayerManager.GetAllPlayerWithTurn()[GetCurrentPlayer()])) NextTurn();
    }
}
