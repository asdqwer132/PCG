using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    protected int turn = 0;
    protected Player currentPlayer;

    public int GetCurrentPlayer() { return currentPlayer.Index; }
    public bool IsMyTurn() { return turn == PlayerManager.GetPlayerOwn().Index; }
    public virtual void SetStartTurn(int value) { turn = value; }
    public virtual void NextTurn()
    {
        if (turn > 0) turn--;
        else turn = PlayerManager.MaxPlayer - 1;
        currentPlayer = PlayerManager.GetAllPlayerWithTurn()[turn];
    }
}
