using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{

    int index;
   [SerializeField]  protected List<CardInfo> handCards = new List<CardInfo>();//
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
    [PunRPC]
    protected void  SetTeam(string newTeam)
    {
        Team Steam = (Team)Enum.Parse(typeof(Team), newTeam);
        team = Steam;
    }
    [PunRPC]
    protected void UseCard(string cardInfos)
    {
        List<CardInfo> useCards = SerializeManager.Deserialize<List<CardInfo>>(cardInfos);
        foreach (CardInfo card in useCards)
        {
            handCards.RemoveAt(handCards.FindIndex(x => x.number == card.number && x.cardType == card.cardType));
        }
    }
    [PunRPC]
    protected void GetCard(string cardInfos)
    {
        List<CardInfo> useCards = SerializeManager.Deserialize<List<CardInfo>>(cardInfos);
        foreach (CardInfo card in useCards)
        {
            handCards.Add(card);
        }
        handCards.Sort((x, y) => x.cardType.CompareTo(y.cardType));
        handCards.Sort((x, y) => x.number.CompareTo(y.number));
    }
    [PunRPC]
    protected void ResetCard()
    {
        handCards = new List<CardInfo>();
    }
}
