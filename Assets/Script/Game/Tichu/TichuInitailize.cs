using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class TichuInitailize : MonoBehaviour, IInitailizer
{
    [SerializeField] List<CardDisplayer> cardDisplayers = new List<CardDisplayer>();
    [SerializeField] List<TextMeshProUGUI> swapnicknames = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> swapDragonNicknames = new List<TextMeshProUGUI>();
    [SerializeField] CardChecker cardChecker;
    [SerializeField] ScoreManager scoreManager;
    public void Intialize(List<Player> sortedPlayer)
    {
        scoreManager.Setup(sortedPlayer);
        cardChecker.Setup(sortedPlayer[0]);
        for (int i = 1; i < sortedPlayer.Count; i++)
        {
            string nickname = sortedPlayer[i].GetComponent<PhotonView>().Owner.NickName;
            TichuPlayer tichuPlayer = (TichuPlayer)sortedPlayer[i];
            cardDisplayers[i - 1].Setup(nickname, tichuPlayer);
            swapnicknames[i - 1].text = nickname;
        }
        swapDragonNicknames[0].text = sortedPlayer[1].GetComponent<PhotonView>().Owner.NickName;
        swapDragonNicknames[1].text = sortedPlayer[3].GetComponent<PhotonView>().Owner.NickName;
    }
}
