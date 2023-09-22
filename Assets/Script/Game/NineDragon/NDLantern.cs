using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDLantern : MonoBehaviour
{
    [SerializeField] Lantern enemy;
    [SerializeField] Lantern player;
    [SerializeField] GameObject enemyFirstMarker;
    [SerializeField] GameObject playerFirstMarker;
    bool isPlayerActive;

    public void SetFirstPlayer(int distance)
    {
        isPlayerActive = distance == 0 ? true : false;
        //setting
        playerFirstMarker.SetActive(isPlayerActive);
        player.SetColor(ColorSelector.OnOffColor(isPlayerActive));
        enemyFirstMarker.SetActive(!isPlayerActive);
        enemy.SetColor(ColorSelector.OnOffColor(!isPlayerActive));
    }
    public void SetLantern()
    {
        isPlayerActive = !isPlayerActive;
        player.SetColor(ColorSelector.OnOffColor(isPlayerActive));
        enemy.SetColor(ColorSelector.OnOffColor(!isPlayerActive));
    }
}
