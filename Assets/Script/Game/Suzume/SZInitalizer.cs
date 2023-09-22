using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SZInitalizer : MonoBehaviour, IInitailizer
{
    [SerializeField] List<SZPlayerDisplay> otherPlayers = new List<SZPlayerDisplay>();
    public void Intialize(List<Player> sortedPlayers)
    {
        for(int i = 5; i > sortedPlayers.Count; i--) { otherPlayers[i - 1].gameObject.SetActive(false); }
        for(int i = 0; i < sortedPlayers.Count; i++) { otherPlayers[i].Setup((SuzumePlayer)sortedPlayers[i], sortedPlayers[i].GetComponent<PhotonView>().Owner.NickName); }
    }
}
