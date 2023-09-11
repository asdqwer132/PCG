using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] CompleteManager completeManager;
    [SerializeField] RoundResultScorePopup roundResultScorePopup;
    [SerializeField] List<GameObject> scores = new List<GameObject>();
    [SerializeField] TextMeshProUGUI redTotalScoreText;
    [SerializeField] TextMeshProUGUI blueTotalScoreText;
    List<TextMeshProUGUI> redTexts = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> blueTexts = new List<TextMeshProUGUI>();
    int round = 0;
    int redTotalScore = 0;
    int blueTotalScore = 0;
    int redScore = 0;
    int blueScore = 0;
    Dictionary<TichuPlayer, int> temporaryScore = new Dictionary<TichuPlayer, int>();
    public int GetTotalScore(Team team)
    {
        if (team == Team.red) return redTotalScore;
        if (team == Team.blue) return blueTotalScore;
        return -1;
    }
    private void Start()
    {
        foreach(GameObject score in scores)
        {
            redTexts.Add(score.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
            blueTexts.Add(score.transform.GetChild(2).GetComponent<TextMeshProUGUI>());
            score.SetActive(false);
        }
        scores[0].SetActive(true);
    }
    public void Setup(List<Player> players)
    {
        foreach (Player player in players)
        {
            TichuPlayer tichuPlayer = (TichuPlayer)player;
            temporaryScore.Add(tichuPlayer, 0);
        }
        ResetTemporaryScore();
    }
    public void FinishRound()
    {
        CalculateRound();
        CalculateTichu();
        SetText();
        roundResultScorePopup.Setup(redScore,blueScore);
        scores[round++].SetActive(true);
    }
    public void AddTemporaryScore(int score,int playerIndex)
    {
        TichuPlayer player = (TichuPlayer)TichuPlayerManager.GetAllPlayerWithTurn()[playerIndex];
        temporaryScore[player] += score;
    }
    void CalculateTichu()
    {
        foreach(TichuPlayer player in TichuPlayerManager.GetAllPlayerWithTurn())
        {
            if(player.Tichu == Tichu.largeTichu || player.Tichu == Tichu.smallTichu)
            {
                int score = player.Tichu == Tichu.largeTichu ? 200 : 100; 
                if (player.Team == Team.red) redScore += completeManager.IsFirstPlayer(player) ? score : -score;
                if (player.Team == Team.blue) blueScore += completeManager.IsFirstPlayer(player) ? score : -score;
            }
        }
        redTotalScore += redScore;
        blueTotalScore += blueScore;
    }
    void CalculateRound()
    {
        redScore = 0;
        blueScore = 0;
        if (completeManager.HowToEnd() == RoundType.oneTwo)
        {
            Debug.Log(completeManager.GetFirstPlayer().Team + "¿øÅõ ÆÀ");
            if (completeManager.GetFirstPlayer().Team == Team.red) redScore += 200;
            if (completeManager.GetFirstPlayer().Team == Team.blue) blueScore += 200;
        }
        else
        {
            foreach (KeyValuePair<TichuPlayer, int> entry in temporaryScore)
            {
                if (completeManager.CheckComplete(entry.Key))
                {
                    if (entry.Key.Team == Team.red) redScore += entry.Value;
                    if (entry.Key.Team == Team.blue) blueScore += entry.Value;
                }
                else
                {
                    int lastScore = ScoreCalculater.CalculateScore(entry.Key.HandCards);
                    if (entry.Key.Team == Team.red) blueScore += entry.Value + lastScore;
                    if (entry.Key.Team == Team.blue) redScore += entry.Value + lastScore;
                }
            }
        }
        ResetTemporaryScore();
    }
    void SetText()
    {
        redTexts[round].text = "" + redScore;
        blueTexts[round].text = "" + blueScore;
        redTotalScoreText.text = "T:" + redTotalScore;
        blueTotalScoreText.text = "T:" + blueTotalScore;
    }
    void ResetTemporaryScore()
    {
        List<TichuPlayer> players = new List<TichuPlayer>();
        foreach (TichuPlayer entry in temporaryScore.Keys) { players.Add(entry); }
        for (int i = 0; i < players.Count; i++) { temporaryScore[players[i]] = 0; }
    }
}
