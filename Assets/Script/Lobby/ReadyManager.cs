using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ReadyManager : MonoBehaviour
{
    int index;
    bool isReady;
    LobbyPlayer player;
    PhotonView playerPV;
    PhotonView PV;
    [SerializeField] TextMeshProUGUI nickname;
    [SerializeField] Image readyBtn;
    [SerializeField] Image teamBtn;

    public bool IsReady()
    {
        if (player != null) return player.GetReady();
        return true;
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        readyBtn.gameObject.GetComponent<Button>().interactable = false;
        teamBtn.gameObject.GetComponent<Button>().interactable = false;
    }
    public void Setup(GameObject playerObj,int index)
    {
        this.index = index;
        readyBtn.gameObject.GetComponent<Button>().interactable = true;
        teamBtn.gameObject.GetComponent<Button>().interactable = true;
        playerPV = playerObj.GetPhotonView(); ;
        player = playerObj.GetComponent<LobbyPlayer>();
        nickname.text = playerPV.Owner.NickName;
        teamBtn.color = ColorSelector.TeamColor(player.GetTeam().ToString());
        readyBtn.color = ColorSelector.OnOffColor(player.GetReady());
        TeamManager.Instance.SetTeam(index, player.GetTeam());
    }
    public void TryReady()
    {
        if (playerPV.IsMine)
        {
            isReady = !isReady;
            player.TrySetReady(isReady);
            PV.RPC("ToggleReady", RpcTarget.All, player.GetReady());
        }
    }
    public void TrySetTeam()
    {
        if (playerPV.IsMine)
        {
            if (player.GetTeam() == Team.random)
            {
                player.TrySetTeam(Team.red);
            }
            else if (player.GetTeam() == Team.red)
            {
                player.TrySetTeam(Team.blue);
            }
            else if (player.GetTeam() == Team.blue)
            {
                player.TrySetTeam(Team.random);
            }
            PV.RPC("SetTeamColor", RpcTarget.All, player.GetTeam().ToString());
        }
    }
    public void TrySortTeam(int maxPlayer) { PV.RPC("SortTeam", RpcTarget.All, maxPlayer); }
    [PunRPC]
    void SortTeam(int maxPlayer)
    {
        TeamManager.Instance.SortTeam(maxPlayer);
    }
    [PunRPC]
    void SetTeamColor(string team)
    {
        teamBtn.color = ColorSelector.TeamColor(team);
        TeamManager.Instance.SetTeam(index, player.GetTeam());
    }
    [PunRPC]
    void ToggleReady(bool isOn)
    {
        readyBtn.color = ColorSelector.OnOffColor(isOn);
    }
}
