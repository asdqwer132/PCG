using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SZPlayerDisplay : MonoBehaviour
{
    [SerializeField] CardListDisplayer cardList;
    [SerializeField] GameObject firstMark;
    [SerializeField] TextMeshProUGUI nicknameText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] SuzumePlayer thisPlayer;//
    public SuzumePlayer GetPlayer { get => thisPlayer; } 
    public bool IsSetted() { return thisPlayer != null; }
    public void Setup(SuzumePlayer player, string nickname)
    {
        thisPlayer = player;
        nicknameText.text = nickname;
    }
    public void SetScore()
    {
         scoreText.text = thisPlayer.GetScore.ToString();

    }
    public void SetDropCardList()
    {
        cardList.SetCard(thisPlayer.GetDropCard);
    }
    public void SetFirstMark(int index) { firstMark.SetActive(thisPlayer.Index == index); }
    public void ResetPlayer()
    {
        thisPlayer.TryResetCard();
        thisPlayer.TryResetDropCards();
    }
    public void ResetInfo()
    {
        scoreText.text = thisPlayer.GetScore.ToString();
    }
}
