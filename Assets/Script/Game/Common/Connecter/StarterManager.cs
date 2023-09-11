using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Photon.Pun;
public class StarterManager : MonoBehaviour
{
    [SerializeField] List<CardDisplayer> cardDisplayers = new List<CardDisplayer>();
    [SerializeField] List<TextMeshProUGUI> swapnicknames = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> swapDragonNicknames = new List<TextMeshProUGUI>();
    [SerializeField] CardChecker cardChecker;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] TichuGameManager gameManager;
    [SerializeField] GameObject waitingPannel;
    [SerializeField] List<Player> players = new List<Player>();
    PhotonView PV;
    private void Awake()
    {
        PhotonNetwork.Instantiate("playerPrefab", Vector2.zero, Quaternion.identity);
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    public void TryGameStart()
    {
        PV.RPC("GameStart", RpcTarget.All);
        gameManager.GameStart();
    }
    [PunRPC]
    void GameStart()
    {
        waitingPannel.SetActive(false);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> ppv = new List<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            ppv.Add(players[i]);
        }
        ppv.Sort((x, y) => x.GetComponent<PhotonView>().ViewID.CompareTo(y.GetComponent<PhotonView>().ViewID));
        foreach (GameObject player in ppv)
        {
            this.players.Add(player.GetComponent<TichuPlayer>());
        }
        SetPlayerTeam();
        SetPlayerTurn();
        SetPlayers();
    }
    void SetPlayers()
    {
        List<Player> sortedPlayer = new List<Player>(); //
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject.GetComponent<PhotonView>().IsMine)
            {
                TichuPlayerManager.SetPlayerOwn(players[i]);
                sortedPlayer = SortPlayerDistance.SortListByReference(players, i);
                break;
            }
        }
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
    void SetPlayerTeam()
    {
        for (int i = 0;i < players.Count;i++)
        {
            Debug.Log(i + "team" + TeamManager.Instance.GetTeam(i));
            players[i].Team = TeamManager.Instance.GetTeam(i);
        }
    }
    void SetPlayerTurn()
    {
        List<Player> redTeamPlayers = new List<Player>();
        List<Player> blueTeamPlayers = new List<Player>();
        foreach (Player player in players)//ÆÀº°·Î ³ª´©±â
        {
            if (player.Team == Team.red) redTeamPlayers.Add(player);
            if (player.Team == Team.blue) blueTeamPlayers.Add(player);
        }
        //ÆÀÀ» ¶ç¾ö¶ç¾ö Á¤·Ä
        List<Player> sortedPlayers = new List<Player>();
        sortedPlayers.Add(redTeamPlayers[0]);
        sortedPlayers.Add(blueTeamPlayers[0]);
        sortedPlayers.Add(redTeamPlayers[1]);
        sortedPlayers.Add(blueTeamPlayers[1]);
        //ÅÏ ¼¼ÆÃ
        for (int i=0;i< sortedPlayers.Count; i++)
        {
            sortedPlayers[i].Index = i;
        }
        players.Sort((x, y) => x.Index.CompareTo(y.Index));
        TichuPlayerManager.SetPlayers(players);
    }
}
