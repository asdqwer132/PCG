using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SuzumePlayer : Player
{
    List<CardInfo> dropCards = new List<CardInfo>();
    int score = 40;

    public List<CardInfo> GetDropCard { get => dropCards; }
    public int GetScore { get => score; }
    public void TrySetScore(bool isPlus, int score) { PV.RPC("SetScore", RpcTarget.All, isPlus, score); }
    public void TryDropCard(CardInfo cardInfo) { PV.RPC("DropCard", RpcTarget.All, SerializeManager.Serialize(cardInfo)); TryUseCard(new List<CardInfo>() { cardInfo }); }
    public void TryResetDropCards() { PV.RPC("ResetDropCards", RpcTarget.All); }
    [PunRPC]
    void SetScore(bool isPlus, int score)
    {
        this.score += isPlus ? score : -score;
    }
    [PunRPC]
    void DropCard(string cardInfo)
    {
        CardInfo dropCard = SerializeManager.Deserialize<CardInfo>(cardInfo);
        this.dropCards.Add(dropCard);
    }
    [PunRPC]
    void ResetDropCards()
    {
        dropCards = new List<CardInfo>();
    }
}
