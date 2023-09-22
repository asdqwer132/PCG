using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDCardDistributer : MonoBehaviour
{
    public void Distribute(List<Player> players)
    {
        foreach(Player player in players)
        {
            player.TryGetCard(GetCardList(player.Team));
        }
    }
    List<CardInfo> GetCardList(Team team)
    {
        string cardColor = team == Team.Red ? CardType.Red.ToString() : CardType.Blue.ToString();
        List<CardInfo> cardInfos = new List<CardInfo>();
        for (int i = 1; i < 10 ; i++)
        {
            cardInfos.Add(new CardInfo(cardColor, i));
        }
        return cardInfos;
    }
}
