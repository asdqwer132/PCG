using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteManager : MonoBehaviour
{
   [SerializeField]  List<Player> completePlayers = new List<Player>();//
    RoundType roundType = RoundType.notYet;
    public bool CheckComplete(Player player) { return completePlayers.Contains(player); }
    public bool IsFirstPlayer(Player player) { return completePlayers[0] == player; }
    public bool IsRoundOver() { return roundType != RoundType.notYet; }
    public int GetCompletedCount() { return completePlayers.Count; }
    public RoundType HowToEnd() { return roundType; }
    public Player GetFirstPlayer() { return completePlayers[0]; }
    public void ResetAll() { roundType = RoundType.notYet; completePlayers = new List<Player>(); }
    public void Complete(Player currentPlayer)
    {
        Player player = currentPlayer;
        if (completePlayers.Count == 1) { if (completePlayers[0].Team == player.Team) { roundType = RoundType.oneTwo; } }
        if (completePlayers.Count == 2) { roundType = RoundType.roundOver; }
        completePlayers.Add(player);
    }
}
