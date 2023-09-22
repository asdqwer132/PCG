using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SZCompleteManager : MonoBehaviour
{
    [SerializeField] List<Player> ronWaitPlayers = new List<Player>();
    [SerializeField] SuzumePlayer targetPlayer;
    [SerializeField] List<SuzumePlayer> ronDeclarePlayers = new List<SuzumePlayer>();
    public List<SuzumePlayer> GetRonDeclarePlayers()
    {
        ronDeclarePlayers.Sort((x, y) => DistanceCalculater.GetDistanceWithRoot(targetPlayer.Index, x.Index).CompareTo(DistanceCalculater.GetDistanceWithRoot(targetPlayer.Index, y.Index)));
        return ronDeclarePlayers;
    }
    public bool IsNotDroped(CardInfo currentDrop, SuzumePlayer player)
    {
        foreach(CardInfo cardInfo in player.GetDropCard)
        {
            if (cardInfo.number == currentDrop.number) return false;
        }
        return true;
    }
    public SuzumePlayer GetRonTargetPlayers() { return targetPlayer; }
    public bool IsWaiting() { return ronWaitPlayers.Count > 0; }
    public void ResetRonPlayer() { ronDeclarePlayers = new List<SuzumePlayer>(); targetPlayer = null; }
    public void DeclareRon(SuzumePlayer player) { ronDeclarePlayers.Add(player); }
    public void SetWaiting(bool plus, Player player, SuzumePlayer target) 
    {
        if (plus)
        {
            targetPlayer = target;
            ronWaitPlayers.Add(player);
        }
        else ronWaitPlayers.Remove(player);
    }
}
