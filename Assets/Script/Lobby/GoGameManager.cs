using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGameManager : MonoBehaviour
{
    [SerializeField] TmpAlpha errorMessage;
    [SerializeField] List<ReadyManager> readyManagers;
    [SerializeField] NetworkManager networkManager;

    private void Start()
    {
        networkManager.Connect();
    }
    public void GoGame(string game)
    {
        for (int i = 0; i < readyManagers.Count; i++)
        {
            if (!readyManagers[i].IsReady())
            {
                errorMessage.FadeOut(0);//���� Ȯ����
                return;//���� ��
            }
        }
        int playerCount = 0;
        for (int i = 0; i < readyManagers.Count; i++)
        {
            playerCount += readyManagers[i].HasPlayer() ? 1 : 0;
        }
        //���� ����
        if(playerCount < GetMinPlayer(game) || playerCount > GetMaxPlayer(game))
        {
            errorMessage.FadeOut(1);//�ο� Ȯ�� ��
            return;//�ο� Ȯ�� ��
        }
        readyManagers[0].TrySortTeam(GetMaxPlayer(game));
        networkManager.LoadArena(game);
    }
    int GetMinPlayer(string game)
    {
        if (game == GameType.Tichu_4p.ToString()) return 4;
        if (game == GameType.NineDragon.ToString()) return 2;
        if (game == GameType.Suzume.ToString()) return 2;
        return 0;
    }
    int GetMaxPlayer(string game)
    {
        if (game == GameType.Tichu_4p.ToString()) return 4;
        if (game == GameType.NineDragon.ToString()) return 2;
        if (game == GameType.Suzume.ToString()) return 5;
        return 0;
    }
}
