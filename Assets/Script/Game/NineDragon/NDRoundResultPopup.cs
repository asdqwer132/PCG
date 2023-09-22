using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public struct NDRoundInfo
{
    public List<NDSubmit> submits;
    public Winner winner;

    public NDRoundInfo(List<NDSubmit> submits, Winner winner)
    {
        this.submits = submits;
        this.winner = winner;
    }
}
public class NDRoundResultPopup : Popup
{
    [SerializeField] List<NDRound> rounds = new List<NDRound>();
    [SerializeField] int round = 0;//
    [SerializeField] bool isAboutVicory;
    List<Winner> winners = new List<Winner>();
    List<NDRoundInfo> totalSubmit = new List<NDRoundInfo>();
    int maxRound = 9;
    private void Start()
    {
        gameObject.SetActive(isAboutVicory);
    }
    public List<NDRoundInfo> GetAllRound() { return totalSubmit; }
    public void SetRound(List<NDSubmit> submits, Winner winner)
    {
        totalSubmit.Add(new NDRoundInfo(submits, winner));
        winners.Add(winner);
        rounds[round].SetRound(submits, winner, isAboutVicory);
        if (round < maxRound) round++;
    }
    public Winner CalculateWinner()
    {
        int redWin = winners.Where(x => x.Equals(Winner.Red)).Count();
        int blueWin = winners.Where(x => x.Equals(Winner.Blue)).Count();
        int draw = winners.Where(x => x.Equals(Winner.Draw)).Count();
        if (redWin + blueWin + draw == 9)//½ÂÀÚ °è»ê
        {
            if (redWin == blueWin) return Winner.Draw;
            Winner winner = redWin > blueWin ? Winner.Red : Winner.Blue;
            return winner;
        }

        return Winner.none;
    }
}
