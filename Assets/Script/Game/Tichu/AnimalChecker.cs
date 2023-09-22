using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalChecker
{
    public static bool CheckAnimal(Animal animal, List<CardInfo> cardInfos)
    {
        foreach(CardInfo cardInfo in cardInfos)
        {
            if (cardInfo.cardType == animal.ToString()) return true;
        }
        return false;
    }
}
