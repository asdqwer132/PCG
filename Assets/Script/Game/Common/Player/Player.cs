using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{

    int index;
    protected List<CardInfo> handCards = new List<CardInfo>();
    protected Team team;
    protected PhotonView PV;
    private void Awake() { PV = GetComponent<PhotonView>(); }
    public Team Team { get => team; set => team = value; }
    public int Index { get => index; set => index = value; }
    public List<CardInfo> HandCards { get => handCards; set => handCards = value; }
    public void TrySetTeam(Team team) { PV.RPC("SetTeam", RpcTarget.AllBuffered, team.ToString()); }
    public void TryResetCard() { PV.RPC("ResetCard", RpcTarget.All); }
    public void TryUseCard(List<CardInfo> cardInfos) { PV.RPC("UseCard", RpcTarget.All, SerializeManager.Serialize(cardInfos)); }
    public void TryGetCard(List<CardInfo> cardInfos) { PV.RPC("GetCard", RpcTarget.All, SerializeManager.Serialize(cardInfos)); }
}
