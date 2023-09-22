using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SZScoreManager : MonoBehaviour
{
    public void CalculateRonScore(int score, SuzumePlayer targetPlayer, SuzumePlayer ronPlayers, CardInfo dropCard)
    {
        int resultScore = targetPlayer.GetScore - score >= 0 ? score : targetPlayer.GetScore;
        targetPlayer.TrySetScore(false, resultScore);
        ronPlayers.TrySetScore(true, resultScore);
    }
    public void CalculateTsumoScore(int score, SuzumePlayer tsumoPlayer, List<SuzumePlayer> otherPlayers)
    {
        int dividedScore = (int)Mathf.Round((float)score / (float)(otherPlayers.Count)) ;
        foreach (SuzumePlayer otherPlayer in otherPlayers)
        {
            int resultScore = otherPlayer.GetScore - dividedScore >= 0 ? dividedScore : otherPlayer.GetScore;
            otherPlayer.TrySetScore(false, resultScore);
            tsumoPlayer.TrySetScore(true, resultScore);
        }
    }
}
