using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    static TeamManager instance;

    public static TeamManager Instance { get => instance; }
    [SerializeField] Team[] teams = new Team[5] { Team.random, Team.random, Team.random, Team.random, Team.random };
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
        if (maxPlayer == 0) return;//������
        int redTeam = 0;
        int blueTeam = 0;
        foreach(Team team in teams)
        {
            if (team == Team.Blue) blueTeam++;
            if (team == Team.Red) redTeam++;
        }
        if (redTeam + blueTeam == maxPlayer && redTeam == blueTeam && redTeam != 0) return;//���� ������ ����
        teams[0] = Team.Red;
        teams[1] = Team.Blue;
        teams[2] = Team.Red;
        teams[3] = Team.Blue;
        teams[4] = Team.Red;
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
