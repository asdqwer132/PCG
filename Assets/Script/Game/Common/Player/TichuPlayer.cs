using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TichuPlayer : Player
{
    [SerializeField] Tichu tichu;
    [SerializeField] int sendCard = 0;
    [SerializeField] List<CardReciever> recieveCard = new List<CardReciever>();
    public Tichu Tichu { get => tichu; set => tichu = value; }
    public int SendCard { get => sendCard; set => sendCard = value; }
    public List<CardReciever> RecieveCard { get => recieveCard; set => recieveCard = value; }

    public bool IsAllSwap() { return sendCard >= TichuPlayerManager.MaxPlayer - 1 && recieveCard.Count >= TichuPlayerManager.MaxPlayer - 1; }
    public void TryUseCardWithSwap(List<CardInfo> cardInfos)
    {
        PV.RPC("UseCardWithSwap", RpcTarget.All);
        base.TryUseCard(cardInfos);
    }
    public void TrySetTichu(Tichu tichu) { PV.RPC("SetTichu", RpcTarget.All, tichu.ToString()); }
    public void TryReceivieCard(List<CardInfo> cardInfos, int sender) { PV.RPC("ReceivieCard", RpcTarget.All, SerializeManager.Serialize(cardInfos), sender); }
    [PunRPC]
    void UseCardWithSwap()
    {
        sendCard++;
    }
    [PunRPC]
    void SetTichu(string newTichu)
    {
        Tichu tichu = (Tichu)Enum.Parse(typeof(Tichu), newTichu);
        this.tichu = tichu;
        if (tichu == Tichu.none)
        {
            sendCard = 0;
            recieveCard = new List<CardReciever>();
        }
    }
    [PunRPC]
    void ReceivieCard(string card, int playerIndex)
    {
        recieveCard.Add(new CardReciever(SerializeManager.Deserialize<List<CardInfo>>(card)[0], playerIndex));
    }
    [PunRPC]
    void SetTeam(string newTeam)
    {
        Team Steam = (Team)Enum.Parse(typeof(Team), newTeam);
        team = Steam;
    }
    [PunRPC]
    void UseCard(string cardInfos)
    {
        List<CardInfo> useCards = SerializeManager.Deserialize<List<CardInfo>>(cardInfos);
        foreach (CardInfo card in useCards)
        {
            handCards.RemoveAt(handCards.FindIndex(x => x.number == card.number && x.cardType == card.cardType));
        }
    }
    [PunRPC]
    void GetCard(string cardInfos)
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
    void ResetCard()
    {
        handCards = new List<CardInfo>();
    }
}
