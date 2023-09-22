using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropManager : MonoBehaviour
{
    CardInfo currentDrop;
    int droperIndex;
    [SerializeField] Card card;
    [SerializeField] Card doraCard;
    [SerializeField] Card DropShowCard;
    [SerializeField] CardDrop cardDrop;
    [SerializeField] GameObject dropBtn;

    public CardInfo CurrentDrop { get => currentDrop; set => currentDrop = value; }
    public CardInfo GetDropCardInfo() { return cardDrop.GetCardInfo(); }
    public CardInfo GetDoraCard() { return doraCard.CardInfo; }
    public void SetDora(CardInfo card) { doraCard.Setup(card); }
    public bool IsOwnDrop() { return droperIndex == PlayerManager.GetPlayerOwn().Index; }
    public bool IsDropSetted() { return cardDrop.GetCardInfo().number != 0; }
    public bool IsDroped(List<CardInfo> cardInfos)
    {
        foreach(CardInfo cardInfo in cardInfos)
        {
            if (GetDropCardInfo() == cardInfo) return true;
        }
        return false;
    }
    public void ResetDrop()
    {
        droperIndex = -1;
        DropShowCard.gameObject.SetActive(false);
        currentDrop = null;
    }
    public void SetCard(CardInfo cardInfo)
    {
        card.gameObject.GetComponent<Image>().color = Color.white;
        card.Setup(cardInfo);
    }
    public void SetDropCard(CardInfo cardInfo,int index)
    {
        DropShowCard.gameObject.SetActive(true);
        DropShowCard.Setup(cardInfo);
        droperIndex = index;
        currentDrop = cardInfo;
    }
    public void HideDropCard()
    {
        SetDropBtn(false);
        cardDrop.ResetCard();
        SetDropArea(false);
    }
    public void SetDropArea(bool isOn)
    {
        cardDrop.gameObject.SetActive(isOn);
        SetDropBtn(isOn);
        cardDrop.ActiveDrop();
    }
    public void SetDropBtn(bool isOn) { dropBtn.SetActive(isOn); }
}
