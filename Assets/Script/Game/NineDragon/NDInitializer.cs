using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NDInitializer : MonoBehaviour, IInitailizer
{
    [SerializeField] TextChanger player;
    [SerializeField] TextChanger enemy;
    public void Intialize(List<Player> players)
    {
        player.SetText(players[0].gameObject.GetComponent<PhotonView>().Owner.NickName);
        player.SetColor(ColorSelector.TeamColor(players[0].Team.ToString()));
        enemy.SetText(players[1].gameObject.GetComponent<PhotonView>().Owner.NickName);
        enemy.SetColor(ColorSelector.TeamColor(players[1].Team.ToString()));
    }
}
