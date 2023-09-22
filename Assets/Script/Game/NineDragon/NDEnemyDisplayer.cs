using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDEnemyDisplayer : MonoBehaviour
{
    [SerializeField] Lantern enemySubmitCard;
    [SerializeField] CardListDisplayer cardList;
    [SerializeField] List<Lantern> lanterns = new List<Lantern>();
    public void SetLanern(List<CardInfo> cardInfos)
    {
        cardList.SetCard(cardInfos);
        if (cardInfos.Count != lanterns.Count) Debug.LogError("not Count match" + name);
        for(int i = 0;i < cardInfos.Count; i++)
        {
            lanterns[i].SetImage(CardImageConverter.instance.GetSprite(EvenConverter.GetIndexByIsEven(cardInfos[i].number), cardInfos[i].cardType));
        }
    }
    public void HideCard(int index) { lanterns[index].gameObject.SetActive(false); }
    public void SetSubmitLantern(int rank, string cardType) { enemySubmitCard.gameObject.SetActive(true); enemySubmitCard.SetImage(CardImageConverter.instance.GetSprite(EvenConverter.GetIndexByIsEven(rank), cardType)); HideCard(rank - 1); }
    public void ResetSubmitLantern() { enemySubmitCard.gameObject.SetActive(false); }

}
