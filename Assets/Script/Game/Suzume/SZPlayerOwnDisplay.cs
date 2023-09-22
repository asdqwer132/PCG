using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SZPlayerOwnDisplay : SZPlayerDisplay
{
    [SerializeField] CardListDisplayer displayer;
    [SerializeField] List<CardDrop> cardDrops;
    [SerializeField] TextMeshProUGUI currentScore;
    [SerializeField] GameObject drawBtn;
    [SerializeField] GameObject tsumoBtn;
    [SerializeField] GameObject ronBtn;
    [SerializeField] GameObject passBtn;
    public void ResetDeclareBtn()
    {
        SetTsumoBtn(false);
        SetRonBtn(false);
        SetCurrentScore(0);
    }
    public void SetTsumoBtn(bool isOn) { tsumoBtn.SetActive(isOn); }
    public void SetPassBtn(bool isOn) { passBtn.SetActive(isOn); }
    public void SetDrawBtn(bool isOn) { drawBtn.SetActive(isOn); }
    public void SetRonBtn(bool isOn) { ronBtn.SetActive(isOn); }
    public void SetCurrentScore(int score) { currentScore.text = score.ToString(); }
    public void Setup(List<CardInfo> cardInfos)
    {
        displayer.SetCardJustInfo(cardInfos);
        int i = 0;
        for(; i< cardInfos.Count; i++)
        {
            cardDrops[i].ActiveDrop();
        }
        for (; i < cardDrops.Count; i++)
        {
            cardDrops[i].ResetCard();
        }
    }
}
