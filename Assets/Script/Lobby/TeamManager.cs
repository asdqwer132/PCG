using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    static TeamManager instance;

    public static TeamManager Instance { get => instance; }
    [SerializeField] Team[] teams = new Team[4] { Team.random, Team.random, Team.random, Team.random };
    public Team GetTeam(int index)
    {
        return teams[index];
    }
    public void SetTeam(int index, Team team)
    {
        teams[index] = team;
    }
    public void SortTeam(int maxPlayer)
    {
        if (maxPlayer == 0) return;//개인전
        int redTeam = 0;
        int blueTeam = 0;
        foreach(Team team in teams)
        {
            if (team == Team.blue) blueTeam++;
            if (team == Team.red) redTeam++;
        }
        if (redTeam + blueTeam == maxPlayer && redTeam == blueTeam && redTeam != 0) return;//팀의 균형이 맞음
        teams[0] = Team.red;
        teams[1] = Team.blue;
        teams[2] = Team.red;
        teams[3] = Team.blue;
    }
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
