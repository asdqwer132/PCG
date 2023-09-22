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
    [SerializeField] NetworkManager networkManager;
    public bool IsEnd() { return scoreManager.GetTotalScore(Team.Red) >= 1000 || scoreManager.GetTotalScore(Team.Blue) >= 1000; }
    public void GameOver()
    {
        Toggle();
        scores.SetActive(true);
        scores.transform.SetParent(scoreTransform);
        scores.transform.localPosition = Vector3.zero;
        if (scoreManager.GetTotalScore(Team.Red) >= 1000)
        {
            if (scoreManager.GetTotalScore(Team.Blue) >= scoreManager.GetTotalScore(Team.Red)) SetWiner(Team.Blue);
            else SetWiner(Team.Red);
        }
        else SetWiner(Team.Blue);
    }
    void SetWiner(Team team)
    {
        title.color = ColorSelector.TeamColor(team.ToString());
        title.text = team == Team.Red ? "Red WIn!" : "Blue Win!";
    }
    public void EndGame()
    {
        networkManager.Disconnect();
    }
}
