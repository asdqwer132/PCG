using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
class Sprites{ public List<Sprite> sprites = new List<Sprite>(); }
public class CardImageConverter : MonoBehaviour
{
    [SerializeField] GameType gameType;
    [SerializeField] List<CardType> cardTypes = new List<CardType>();
    [SerializeField] List<Sprites> sprites = new List<Sprites>();
    Dictionary<CardType, List<Sprite>> cardSprites = new Dictionary<CardType, List<Sprite>>();
    public static CardImageConverter instance;
    private void OnDestroy()
    {
        instance = null;
    }
    private void Start()
    {
        if (instance == null) instance = this;
        if (cardTypes.Count != sprites.Count)
        {
            Debug.LogError("Count not matching");
            return;
        }
        for(int i = 0;i < cardTypes.Count; i++)
        {
            cardSprites.Add(cardTypes[i], sprites[i].sprites);
        }
        //cardSprites.Add(CardType.Red, red);
        //cardSprites.Add(CardType.Blue, blue);
        //cardSprites.Add(CardType.Green, green);
        //cardSprites.Add(CardType.Yellow, yellow);
        //cardSprites.Add(CardType.Animal, animal);
    }
    public Sprite GetSprite(int rank, string cardColor)
    {
        Sprite sprite = null;
        CardType cardType = GetCardType(cardColor);
        if (cardType != CardType.Animal) sprite = cardSprites[cardType][NumberConverter.RankConverter(gameType, rank)];
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
