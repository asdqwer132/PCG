using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SZRoundManager : RotationManager
{
    [SerializeField] TextMeshProUGUI roundText;
    bool isRoundStarter = true;
    int currentRound = 1;
    int maxRound = 4 + 1;
    public bool IsGameOver() { return currentRound >= maxRound; }
    public bool IsStartPlayer(Player player) { return currentPlayer == player; }
    public bool IsRoundOver() { return IsAllPass() && !isRoundStarter; }
    new public void NextTurn()
    {
        isRoundStarter = false;
        base.NextTurn();
    }
    public void RoundOver()
    {
        isRoundStarter = true;
        currentRound++;
        roundText.text = IsGameOver() ? "GameOver" : currentRound.ToString();
    }
}
