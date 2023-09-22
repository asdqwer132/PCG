using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDTurnManager : TurnManager
{
    [SerializeField] NDLantern lantern;
    public void SetStartTurn(string starter)
    {
        int value = GetStartTurnByWinnet(starter);
        base.SetStartTurn(value);
        lantern.SetFirstPlayer(DistanceCalculater.GetDistance(value));
    }
    public override void NextTurn()
    {
        base.NextTurn();
        lantern.SetLantern();
    }
    int GetStartTurnByWinnet(string winner)
    {
        int index = winner == PlayerManager.GetPlayerWithTurn(0).Team.ToString() ? 0 : 1;
        return index;
    }
}
