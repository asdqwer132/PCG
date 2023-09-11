using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LobbyPlayer : MonoBehaviour
{

    Team team;
    bool isReady;
    PhotonView PV;
    void Awake() { PV = GetComponent<PhotonView>(); }
    public bool GetReady() { return isReady; }
    public Team GetTeam() { return team; }
    public void TrySetReady(bool isOn) { PV.RPC("SetReady", RpcTarget.AllBuffered, isOn); }
    public void TrySetTeam(Team team) { PV.RPC("SetTeam", RpcTarget.AllBuffered, team.ToString()); }
    [PunRPC]
    void SetTeam(string newTeam)
    {
        Team Steam = (Team)Enum.Parse(typeof(Team), newTeam);
        team = Steam;
    }
    [PunRPC]
    void SetReady(bool isOn)
    {
        isReady = isOn;
    }
}
