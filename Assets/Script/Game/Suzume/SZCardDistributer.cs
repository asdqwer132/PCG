using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SZCardDistributer : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI leftCard;
    [SerializeField] List<CardInfo> totalCardList = new List<CardInfo>();//
    public List<CardInfo> TotalCardList { get => totalCardList; set => totalCardList = value; }
    public bool CardAllUsed() { return totalCardList.Count == 0; }
    public void SetLeftCardCount() { leftCard.text = "" + totalCardList.Count; }
    public CardInfo Distribute()
    {
        CardInfo card = totalCardList[0];
        totalCardList.RemoveAt(0);
        return card;
    }
    public List<CardInfo> ResetTotalCardList()
    {

        List<CardInfo> totalCardList = new List<CardInfo>();
        for (int i = 1; i <= 9; i++)
        {
            totalCardList.Add(new CardInfo(CardType.Red.ToString(), i));
        }
        for (int i = 0; i < 3; i++)
        {
            for (int index = 1; index <= 9; index++)
            {
                totalCardList.Add(new CardInfo(CardType.Green.ToString(), index));
            }
        }
        for (int i = 0; i < 4; i++)
        {
            totalCardList.Add(new CardInfo(CardType.Red.ToString(), 13));//Áß
            totalCardList.Add(new CardInfo(CardType.Green.ToString(), 11));//¹ß
        }
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
