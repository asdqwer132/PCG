using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static List<Player> players = new List<Player>();
    static Player playerOwn;
    static int maxPlayer;
    public static int MaxPlayer { get => maxPlayer; }
    public static void ResetPlayersHandCards()
    {
        foreach(Player player in players)
        {
            player.TryResetCard();
        }
    }
    public static void SetPlayers(List<Player> newPlayers) { players = newPlayers; maxPlayer = players.Count; }
    public static void SetPlayerOwn(Player newPlayer) { playerOwn = newPlayer; }
    public static Player GetPlayerOwn() { return playerOwn; }
    public static Player GetPlayerWithDirection(int index) { return SortPlayerDistance.SortListByReference(players, GetPlayerOwn().Index)[index]; }
    public static Player GetPlayerWithTurn(int index) { return players[index]; }
    public static List<Player> GetAllPlayerWithTurn() { return players; }

}
