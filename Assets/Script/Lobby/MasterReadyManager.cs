using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MasterReadyManager : MonoBehaviour
{
    [SerializeField] ReadyManager master;
    [SerializeField] ReadyManager[] readyManagers;
    
    public void Kick(int index)
    {
        if(master.HasPlayer() && readyManagers[index].HasPlayer())
        {
            if (master.GetPlayer().IsMine)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
    }
}
