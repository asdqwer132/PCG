using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    [SerializeField] CardChecker cardChecker;
    Card card;
    bool isSelectced;

    public bool IsSelectced { get => isSelectced; set => isSelectced = value; }

    private void Start()
    {
        card = GetComponent<Card>();
    }
    public void SelectCard()
    {
        isSelectced = !isSelectced;
        gameObject.transform.position += isSelectced ? new Vector3(0, 10, 0) : new Vector3(0, -10, 0);
        if (isSelectced) cardChecker.SelectCard(card.CardInfo);
        else cardChecker.DeleteCard(card.CardInfo);
    }
}
