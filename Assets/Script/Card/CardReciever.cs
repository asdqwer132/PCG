using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardReciever
{
    public CardInfo cardInfo;
    public int playerIndex;

    public CardReciever(CardInfo cardInfo, int playerIndex)
    {
        this.cardInfo = cardInfo;
        this.playerIndex = playerIndex;
    }
}
