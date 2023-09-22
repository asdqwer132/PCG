using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDropPopup : Popup
{
    [SerializeField] CardListDisplayer cardDisplayer;
    [SerializeField] SZPlayerDisplay display;
    public void Setup()
    {
        cardDisplayer.SetCard(display.GetPlayer.GetDropCard);
        Toggle();
    }
}
