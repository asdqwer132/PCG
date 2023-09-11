using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGameManager : MonoBehaviour
{
    [SerializeField] TmpAlpha errorMessage;
    [SerializeField] List<ReadyManager> readyManagers;
    [SerializeField] NetworkManager networkManager;

    public void GoGame(string game)
    {
        for (int i = 0; i < readyManagers.Count; i++)
        {
            if (!readyManagers[i].IsReady())
            {
                errorMessage.FadeOut(0);
                return;//레디 점
            }
        }
        //게임 시작
        readyManagers[0].TrySortTeam(GetMaxPlayer(game));
        networkManager.LoadArena(game);
    }
    int GetMaxPlayer(string game)
    {
        if (game == GameType.Tichu_4p.ToString()) return 4;
        if (game == GameType.NineDragon.ToString()) return 2;
        return 0;
    }
}
