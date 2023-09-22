using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SZTurnManager : TurnManager
{
    [SerializeField] TichuLanternManager turnLantern;
    public void ResetAll() { turnLantern.ResetTurnLanter(); }
    public override void SetStartTurn(int value)
    {
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), false);
        currentPlayer = PlayerManager.GetAllPlayerWithTurn()[value];
        base.SetStartTurn(value);
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), true);
    }
    public override void NextTurn()
    {
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), false);
        base.NextTurn();
        turnLantern.SetTurnLantern(DistanceCalculater.GetDistance(turn), true);
    }
}
