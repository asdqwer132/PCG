using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
public class WaitManager : MonoBehaviour
{
    [SerializeField] List<ReadyManager> playerInfos;
    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected);
        PhotonNetwork.Instantiate("LobbyPlayer", Vector2.zero, Quaternion.identity);
        PV.RPC("SetNickname", RpcTarget.All);
    }
    public void TrySetNickname()
    {
        StartCoroutine("CreatePlayer");
    }
    [PunRPC]
    void SetNickname()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> ppv = new List<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            ppv.Add(players[i]);
        }
        ppv.Sort((x, y) => x.GetComponent<PhotonView>().ViewID.CompareTo(y.GetComponent<PhotonView>().ViewID));
        for (int i = 0; i < ppv.Count; i++)
        {
            playerInfos[i].Setup(ppv[i], i);
        }
    }
}
