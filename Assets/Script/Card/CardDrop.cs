using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrop : MonoBehaviour, IDropHandler
{
    [SerializeField] Card thisCard;//
    [SerializeField] bool DisableDragObject;
    public CardInfo GetCardInfo() { return thisCard.CardInfo; }
    void Start()
    {
        thisCard = thisCard == null ? GetComponent<Card>() : thisCard;
    }
    public void ResetCard()
    {
        thisCard.CardInfo = new CardInfo("none", 0);
     //   SetDrag(false);
        thisCard.GetComponent<Lantern>().SetColor(new Color(1,1,1,0));
    }
    public void ActiveDrop()
    {
      //  SetDrag(true);
        thisCard.GetComponent<Lantern>().SetColor(new Color(1, 1, 1,  1));
    }
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Debug.Log("드랍");
        if (eventData.pointerDrag != null)
        {
            SoundEffecter.Instance.PlayEffectUIByRandom(SoundType.card, "drop", 4);
            Debug.Log("드랍s");
            CardDrag drag = eventData.pointerDrag.GetComponent<CardDrag>();
            Card dragCard = drag.ThisCard;
            thisCard.GetComponent<Lantern>().SetColor(new Color(1, 1, 1, 1));
            if (thisCard.CardInfo.number != 0)//스왑
            {
                SwapCardInfo(dragCard, thisCard);
            }
            else//그냥 넣기
            {
                // drag.enabled = false;
                Debug.Log("그냥 넣기");
               // SetDrag(true);
                thisCard.Setup(dragCard.CardInfo);
                drag.HideCopy();
                if (DisableDragObject) drag.gameObject.SetActive(false);//드래그된 오브젝트 삭제
                else
                {
                    eventData.pointerDrag.GetComponent<CardDrop>().ResetCard();//비워두기
                    Debug.Log("비워두기");
                }
            }
        }
    }
    void SetDrag(bool isOn)
    {
        CardDrag drag = GetComponent<CardDrag>();
        if (drag != null) drag.enabled = isOn;
    }
    void SwapCardInfo(Card from, Card to)
    {
        CardInfo toCard = new CardInfo(to.CardInfo.cardType, to.CardInfo.number);
        CardInfo fromCard = new CardInfo(from.CardInfo.cardType, from.CardInfo.number);
        from.Setup(toCard);
        to.Setup(fromCard);
    }
}