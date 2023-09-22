using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SZVictoryPopup : Popup, IVictoryManagerInteface
{
    [SerializeField] List<SZWinnerDisplayer> displayers = new List<SZWinnerDisplayer>();
    [SerializeField] NetworkManager networkManager;
    public void Setup(List<Player> players)
    {
        Toggle();
        List<SuzumePlayer> suzumePlayers = new List<SuzumePlayer>();
        foreach (Player player in players) { suzumePlayers.Add((SuzumePlayer)player); }
        suzumePlayers.Sort((x, y) => y.GetScore.CompareTo(x.GetScore));
        for(int i =0; i < suzumePlayers.Count; i++)
        {
            string nickname = suzumePlayers[i].gameObject.GetComponent<PhotonView>().Owner.NickName;
            displayers[i].gameObject.SetActive(true);
            displayers[i].Setup(suzumePlayers[i].GetScore, nickname);
        }
    }
    public void EndGame()
    {
        networkManager.Disconnect();
    }
}
