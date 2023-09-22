using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
public class StarterManager : MonoBehaviour
{
    [SerializeField] GameType gameType;
    [SerializeField] bool isNoTeam;
    [SerializeField] GameObject waitingPannel;
    [SerializeField] List<Player> players = new List<Player>();//
    IGameManagerInterface gameManager;
    IInitailizer initailizer;
    PhotonView PV;
    private void Awake()
    {
        PhotonNetwork.Instantiate(gameType.ToString() + "Player", Vector2.zero, Quaternion.identity);
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        waitingPannel = GameObject.Find("login");
        initailizer = GameObject.FindWithTag("Initailizer").GetComponent<IInitailizer>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<IGameManagerInterface>();
    }
    public void TryGameStart()
    {
        PV.RPC("GameStart", RpcTarget.All);
        gameManager.TryGameStart();
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
            this.players.Add(player.GetComponent<Player>());
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
                PlayerManager.SetPlayerOwn(players[i]);
                sortedPlayer = SortPlayerDistance.SortListByReference(players, i);
                break;
            }
        }
        initailizer.Intialize(sortedPlayer);
    }
    void SetPlayerTeam()
    {
        for (int i = 0;i < players.Count;i++)
        {
            players[i].Team = TeamManager.Instance.GetTeam(i);
        }
    }
    void SetPlayerTurn()
    {
        List<Player> redTeamPlayers = new List<Player>();
        List<Player> blueTeamPlayers = new List<Player>();
        foreach (Player player in players)//ÆÀº°·Î ³ª´©±â
        {
            if (player.Team == Team.Red) redTeamPlayers.Add(player);
            if (player.Team == Team.Blue) blueTeamPlayers.Add(player);
        }
        //ÆÀÀ» ¶ç¾ö¶ç¾ö Á¤·Ä
        List<Player> sortedPlayers = new List<Player>();
        if (isNoTeam)
        {
            foreach (Player player in players)//ÆÀº°·Î ³ª´©±â
            {
                sortedPlayers.Add(player);
            }
        }
        else
        {
            sortedPlayers.Add(redTeamPlayers[0]);
            sortedPlayers.Add(blueTeamPlayers[0]);
            if (redTeamPlayers.Count > 1) sortedPlayers.Add(redTeamPlayers[1]);
            if (blueTeamPlayers.Count > 1) sortedPlayers.Add(blueTeamPlayers[1]);
        }
        //ÅÏ ¼¼ÆÃ
        for (int i=0;i< sortedPlayers.Count; i++)
        {
            sortedPlayers[i].Index = i;
        }
        players.Sort((x, y) => x.Index.CompareTo(y.Index));
        PlayerManager.SetPlayers(players);
    }
}
