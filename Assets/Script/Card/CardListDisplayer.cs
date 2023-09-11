using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardListDisplayer : MonoBehaviour
{
    [SerializeField] List<Card> cards = new List<Card>();

    public void ResetActive()
    {
        for(int i = 0;i < cards.Count; i++)
        {
            cards[i].gameObject.SetActive(false);
        }
    }
    public void SetCard(List<CardInfo> cardInfos)
    {
        ResetActive();
        for (int i = 0; i < cardInfos.Count; i++)
        {
            cards[i].gameObject.SetActive(true);
            cards[i].Setup(cardInfos[i]);
        }
    }
    public void ResetAll()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].gameObject.SetActive(false);
            cards[i].gameObject.GetComponent<CardSelector>().IsSelectced = false;
        }
    }
    public void HideCard(CardInfo card)
    {
        int index = cards.FindIndex(x => x.CardInfo == card);
        cards[index].gameObject.SetActive(false);
        cards[index].gameObject.GetComponent<CardSelector>().IsSelectced = false;
    }
}
