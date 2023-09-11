using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    protected int turn = 0;
    protected Player currentPlayer;

    public int GetCurrentPlayer() { return currentPlayer.Index; }
    public bool IsMyTurn() { return turn == TichuPlayerManager.GetPlayerOwn().Index; }
    public virtual void NextTurn()
    {
        if (turn > 0) turn--;
        else turn = TichuPlayerManager.MaxPlayer - 1;
        currentPlayer = TichuPlayerManager.GetAllPlayerWithTurn()[turn];
    }
}
