using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TichuVictoryManager : Popup, IVictoryManagerInteface
{
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] Transform scoreTransform;
    [SerializeField] GameObject scores;
    [SerializeField] TextMeshProUGUI title;
    public bool IsEnd() { return scoreManager.GetTotalScore(Team.red) >= 1000 || scoreManager.GetTotalScore(Team.blue) >= 1000; }
    public void GameOver()
    {
        Toggle();
        scores.SetActive(true);
        scores.transform.SetParent(scoreTransform);
        scores.transform.localPosition = Vector3.zero;
        if (scoreManager.GetTotalScore(Team.red) >= 1000)
        {
            if (scoreManager.GetTotalScore(Team.blue) >= scoreManager.GetTotalScore(Team.red)) SetWiner(Team.blue);
            else SetWiner(Team.red);
        }
        else SetWiner(Team.blue);
    }
    void SetWiner(Team team)
    {
        title.color = ColorSelector.TeamColor(team.ToString());
        title.text = team == Team.red ? "Red WIn!" : "Blue Win!";
    }
}
