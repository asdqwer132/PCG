using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculater
{
    public static int CalculateScore(List<CardInfo> cards)
    {
        int score = 0;
        foreach(CardInfo card in cards)
        {
            if (card.number == (int)Animal.phoenix) score -= 25;//ºÀÈ²
            if (card.number == 5) score += 5;//5
            if (card.number == 10) score += 10;//10
            if (card.number == 13) score += 10;//k
            if (card.number == (int)Animal.phoenix) score += 25;//¿ë
        }
        return score;
    }
    public bool IsDragon(List<CardInfo> cards)
    {
        foreach (CardInfo card in cards)
        {
            if (card.number == 15) return true;
        }
        return false;
    }
}
