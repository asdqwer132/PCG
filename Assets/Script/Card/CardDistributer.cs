using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CardDistributer : MonoBehaviour
{
    [SerializeField] List<CardDisplayer> cardDisplayers = new List<CardDisplayer>();
    [SerializeField] List<CardInfo> totalCardList = new List<CardInfo>();//
    public List<CardInfo> TotalCardList { get => totalCardList; set => totalCardList = value; }

    public void FirstDistribute(List<Player> players)
    {
        foreach (Player player in players)
        {
            player.TryResetCard();
            DistributeWithCount(8, player);
        }
        foreach (CardDisplayer cardDisplayer in cardDisplayers)
        {
            cardDisplayer.SetCard();
        }
    }
    public void SecondDistribute(List<Player> players)
    {
        foreach (Player player in players)
        {
            DistributeWithCount(6, player);
        }
    }
    void DistributeWithCount(int count, Player player)
    {
        List<CardInfo> cardInfos = new List<CardInfo>();
        for (int i = 0; i < count; i++)
        {
            cardInfos.Add(totalCardList[0]);
            totalCardList.RemoveAt(0);
        }
        player.TryGetCard(cardInfos);
    }
    public List<CardInfo> ResetTotalCardList()
    {
        List<CardInfo> totalCardList = new List<CardInfo>();
        List<string> cardTypes = new List<string>() { CardType.Red.ToString(), CardType.Yellow.ToString(), CardType.Green.ToString(), CardType.Blue.ToString() };
        foreach (string cardType in cardTypes)
        {
            for (int i = 2; i <= 14; i++)
            {
                totalCardList.Add(new CardInfo(cardType, i));
            }
        }
        totalCardList.Add(new CardInfo(Animal.phoenix.ToString(), (int)Animal.phoenix)); ;//봉황
        totalCardList.Add(new CardInfo(Animal.bird.ToString(), (int)Animal.bird));//참새
        totalCardList.Add(new CardInfo(Animal.dragon.ToString(), (int)Animal.dragon));//용
        totalCardList.Add(new CardInfo(Animal.dog.ToString(), (int)Animal.dog));//개
        totalCardList = Shuffle(totalCardList);
        return totalCardList;
    }
    List<CardInfo> Shuffle(List<CardInfo> totalCardList)
    {
        List<CardInfo> newCards = new List<CardInfo>();
        int count = totalCardList.Count;
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, totalCardList.Count);
            newCards.Add(totalCardList[index]);
            totalCardList.RemoveAt(index);
        }
        return newCards;
    }
}
