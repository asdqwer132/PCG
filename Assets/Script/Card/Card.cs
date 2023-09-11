using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    CardInfo cardInfo;
    [SerializeField] Image cardFrame;
    [SerializeField] Image cardTypeLogo;
    [SerializeField] TextMeshProUGUI number;

    public CardInfo CardInfo { get => cardInfo; set => cardInfo = value; }
    public void Setup(CardInfo cardInfo)
    {
        this.CardInfo = cardInfo;
        cardFrame.sprite = CardImageConverter.instance.GetSprite(cardInfo.number, cardInfo.cardType);
    }
}
