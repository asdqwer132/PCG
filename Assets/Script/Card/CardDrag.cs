using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] GameObject copy;
    [SerializeField] Card thisCard;//
    [SerializeField] Canvas canvas;
    RectTransform rectTransform;
    Transform _startParent;
    CanvasGroup canvasGroup;

    public Card ThisCard { get => thisCard; set => thisCard = value; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        thisCard = GetComponent<Card>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (thisCard.CardInfo.number == 0) return;
        copy.SetActive(true);
        copy.transform.position = transform.position;
        //_startParent = transform.parent;
        Card card = copy.GetComponent<Card>();
        card.Setup(thisCard.CardInfo);
        rectTransform = copy.GetComponent<RectTransform>();
        copy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
       // index = transform.GetSiblingIndex();
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���� �̵��� ���ؼ� �󸶳� �̵��ߴ����� ������
        // ĵ������ �����ϰ� ����� �ϱ� ������
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        HideCopy();

       // transform.SetParent(_startParent);
       // transform.SetSiblingIndex(index);
      //  transform.localPosition = Vector3.zero;
    }
    public void HideCopy() { copy.SetActive(false); }
}
