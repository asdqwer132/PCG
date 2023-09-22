using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
public struct Hwaryo
{
    public List<CardInfo> player;
    public int score;

    public Hwaryo(List<CardInfo> player, int score)
    {
        this.player = player;
        this.score = score;
    }
} 
public class HwaryoPopup : Popup
{
    List<Hwaryo> completePlayers = new List<Hwaryo>();
    [SerializeField] SZVictoryPopup winnerPopup;
    [SerializeField] CardInfo dropCardInfo;//
    [SerializeField] TextMeshProUGUI howToWin;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Card dropCard;
    [SerializeField] CardListDisplayer displayer;
    public void Setup(int score, List<CardInfo> players, string how, CardInfo dropCard)
    {
        completePlayers.Add(new Hwaryo(CopyCardInfo(players), score));
        howToWin.text = how;
        Debug.Log(how + "/" + dropCard.number);
        dropCardInfo = dropCard;
    }
    public void NextSlot()
    {
        if (completePlayers.Count == 0)
        {
            Close();
            if(SZGameManager.isGameOver) winnerPopup.Setup(PlayerManager.GetAllPlayerWithTurn());
            return;
        }
        if (howToWin.text == "·Ð!") SoundEffecter.Instance.PlayEffect(SoundType.game, "ron");
        if (howToWin.text == "Âê¸ð!") SoundEffecter.Instance.PlayEffect(SoundType.game, "tsumo");
        scoreText.text = "x" + completePlayers[0].score;
        List<CardInfo> cardInfos = completePlayers[0].player;
        cardInfos.Sort((x, y) => x.number.CompareTo(y.number));
        displayer.SetCard(cardInfos);
        //dropCard.transform.SetAsFirstSibling();
        if (dropCardInfo.number != 0)
        {
           // dropCard.transform.SetAsLastSibling();
            dropCard.Setup(dropCardInfo);
        }
        Toggle();
        completePlayers.RemoveAt(0);
    }
    public List<CardInfo> CopyCardInfo(List<CardInfo> copyList)
    {
        List<CardInfo> pasteList = new List<CardInfo>();
        foreach(CardInfo cardInfo in copyList)
        {
            pasteList.Add(new CardInfo(cardInfo.cardType, cardInfo.number));
        }
        return pasteList;
    }
}
