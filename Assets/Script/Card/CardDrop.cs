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
        Debug.Log("���");
        if (eventData.pointerDrag != null)
        {
            SoundEffecter.Instance.PlayEffectUIByRandom(SoundType.card, "drop", 4);
            Debug.Log("���s");
            CardDrag drag = eventData.pointerDrag.GetComponent<CardDrag>();
            Card dragCard = drag.ThisCard;
            thisCard.GetComponent<Lantern>().SetColor(new Color(1, 1, 1, 1));
            if (thisCard.CardInfo.number != 0)//����
            {
                SwapCardInfo(dragCard, thisCard);
            }
            else//�׳� �ֱ�
            {
                // drag.enabled = false;
                Debug.Log("�׳� �ֱ�");
               // SetDrag(true);
                thisCard.Setup(dragCard.CardInfo);
                drag.HideCopy();
                if (DisableDragObject) drag.gameObject.SetActive(false);//�巡�׵� ������Ʈ ����
                else
                {
                    eventData.pointerDrag.GetComponent<CardDrop>().ResetCard();//����α�
                    Debug.Log("����α�");
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