using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NDVictoryPopup : Popup, IVictoryManagerInteface
{
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] NDRoundResultPopup resultPopup;
    [SerializeField] NetworkManager networkManager;
    public void EndGame()
    {
        networkManager.Disconnect();
    }
    public void GameOver(Winner winner, List<NDRoundInfo> submited)
    {
        Toggle();
        winnerText.color = ColorSelector.CardColor(winner.ToString());
        winnerText.text = winner != Winner.Draw ? winner.ToString() + "Win!" : "Draw? Are you serious?";
        foreach (NDRoundInfo submits in submited)
        {
            resultPopup.SetRound(submits.submits, submits.winner);
        }
    }
}
