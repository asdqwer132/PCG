using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardImageConverter : MonoBehaviour
{
    [SerializeField] Sprite[] red;
    [SerializeField] Sprite[] green;
    [SerializeField] Sprite[] blue;
    [SerializeField] Sprite[] yellow;
    [SerializeField] Sprite[] animal;
    Dictionary<CardType, Sprite[]> cardSprites = new Dictionary<CardType, Sprite[]>();
    public static CardImageConverter instance;
    private void OnDestroy()
    {
        instance = null;
    }
    private void Start()
    {
        if (instance == null) instance = this;
        cardSprites.Add(CardType.Red, red);
        cardSprites.Add(CardType.Blue, blue);
        cardSprites.Add(CardType.Green, green);
        cardSprites.Add(CardType.Yellow, yellow);
        cardSprites.Add(CardType.Animal, animal);
    }
    public Sprite GetSprite(int rank, string cardColor)
    {
        Sprite sprite = null;
        CardType cardType = GetCardType(cardColor);
        if (cardType != CardType.Animal) sprite = cardSprites[cardType][rank - 2];
        else
        {
            if (rank == (int)Animal.dragon) sprite = cardSprites[cardType][0];//¿ë
            if (rank == (int)Animal.phoenix) sprite = cardSprites[cardType][1];//ºÀÈ²
            if (rank == (int)Animal.dog) sprite = cardSprites[cardType][2];//°³
            if (rank == (int)Animal.bird) sprite = cardSprites[cardType][3];//»õ
        }
        return sprite;
    }
    CardType GetCardType(string cardColor)
    {
        if (cardColor == "Red") return CardType.Red;
        if (cardColor == "Blue") return CardType.Blue;
        if (cardColor == "Green") return CardType.Green;
        if (cardColor == "Yellow") return CardType.Yellow;
        return CardType.Animal;
    }
}
