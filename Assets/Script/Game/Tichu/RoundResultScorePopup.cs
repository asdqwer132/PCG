using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoundResultScorePopup : Popup
{
    [SerializeField] TextMeshProUGUI red;
    [SerializeField] TextMeshProUGUI blue;
    public void Setup(int redScore, int blueScore)
    {
        Toggle();
        red.text = redScore.ToString();//
        blue.text = blueScore.ToString();
    }
}
