using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : TurnManager
{
    protected Player startPlayer;//
    public bool IsAllPass() { return currentPlayer == startPlayer; }
    public Player GetStartPlayer() { return startPlayer; }
    public override void SetStartTurn(int value)
    {
        currentPlayer = PlayerManager.GetAllPlayerWithTurn()[value];
        startPlayer = PlayerManager.GetAllPlayerWithTurn()[GetCurrentPlayer()];
        base.SetStartTurn(value);
    }
}
