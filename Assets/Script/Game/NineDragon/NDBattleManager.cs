using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NDBattleManager : MonoBehaviour
{

    public Winner WhoIsWinner(NDSubmit player1, NDSubmit player2)
    {
        if (player1.rank == player2.rank) return Winner.Draw;
        if (player1.rank < player2.rank)
        {
            if (player1.rank == 1 && player2.rank == 9) return GetWinnerByTeam(player1.team);
            return GetWinnerByTeam(player2.team);
        }
        else
        {
            if (player1.rank == 9 && player2.rank == 1) return GetWinnerByTeam(player2.team);
            return GetWinnerByTeam(player1.team);
        }
    }
    Winner GetWinnerByTeam(Team team)
    {
        if (team == Team.Blue) return Winner.Blue;
        return Winner.Red;
    }
}