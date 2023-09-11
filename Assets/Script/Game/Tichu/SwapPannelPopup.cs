using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPannelPopup : Popup
{
    [SerializeField] List<Card> cards;
    public void Setup(List<CardReciever> cardInfos)
    {
        if (cards.Count != cardInfos.Count) throw new System.Exception("not count match : SwapPannelPopup => Setup");
        for(int i= 0;i < cardInfos.Count; i++)
        {
            cards[DistanceCalculater.GetDistance(cardInfos[i].playerIndex) - 1].Setup(cardInfos[i].cardInfo);
        }
        Toggle();
    }
}
