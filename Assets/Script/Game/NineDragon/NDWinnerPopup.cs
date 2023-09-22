using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDWinnerPopup : Popup
{
    [SerializeField] TextRepeater text;
    [SerializeField] float term = 2.0f;
    public void OpenPopup(Winner winner)
    {
        Toggle();
        text.SetColor(Color.black);
        text.StartRepeat();
        SoundEffecter.Instance.PlayEffect(SoundType.game, "drum");
        StartCoroutine("SetWinner", winner);
    }

    IEnumerator SetWinner(Winner winner)
    {
        yield return new WaitForSeconds(term);
        SoundEffecter.Instance.PlayEffect(SoundType.game,winner.ToString());
        text.StopRepeat();
        text.SetText(ConvertText(winner));
        text.SetColor(ColorSelector.CardColor(winner.ToString()));
    }
    string ConvertText(Winner winner)
    {
        string text = winner == Winner.Draw ? "Draw" : (winner.ToString() + " Win!");
        return text;
    }
}
