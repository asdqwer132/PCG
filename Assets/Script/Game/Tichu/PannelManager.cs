using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelManager : MonoBehaviour
{
    [SerializeField] TichuGameManager gameManager;
    [SerializeField] SubmitManager submitManager;
    [SerializeField] GameObject largeTichuLogo;
    [SerializeField] GameObject smallTichuLogo;
    [SerializeField] GameObject largeTichuArea;
    [SerializeField] GameObject smallTichuArea;
    [SerializeField] GameObject sendDragonArea;
    [SerializeField] GameObject sendArea;
    [SerializeField] GameObject submitArea;
    [SerializeField] List<GameObject> sendBtns = new List<GameObject>();
    int score = 0;
    private void Start()
    {
        ResetPannel(true);
    }
    #region ActiveArea
    public void ResetPannel(bool withLogoActive)
    {
        if (withLogoActive)
        {
            largeTichuLogo.SetActive(false);
            smallTichuLogo.SetActive(false);
        }
        largeTichuArea.SetActive(false);
        smallTichuArea.SetActive(false);
        sendDragonArea.SetActive(false);
        sendArea.SetActive(false);
        submitArea.SetActive(false);
        foreach(GameObject btn in sendBtns)
        {
            btn.SetActive(true);
        }
    }
    public void ActiveLargeTichuArea(bool isActive)
    {
        ResetPannel(false);
        largeTichuArea.SetActive(isActive);
    }
    public void ActiveSmallTichuArea(bool isActive)
    {
        smallTichuArea.SetActive(isActive);
    }
    public void ActiveSendArea(bool isActive)
    {
        ResetPannel(false);
        sendArea.SetActive(isActive);
        sendArea.transform.localPosition = Vector3.zero;
    }
    public void ActiveSendDragonArea(bool isActive)
    {
        ResetPannel(false);
        sendDragonArea.SetActive(isActive);
    }
    public void ActiveSubmitArea(bool isActive)
    {
        ResetPannel(false);
        submitArea.SetActive(isActive);
    }
    public void ActiveSendBtn(bool isActive, int index)
    {
        sendBtns[index].SetActive(isActive);
    }
    public void ActiveTichuLogo(Tichu tichu)
    {
        if (tichu == Tichu.largeTichu) largeTichuLogo.SetActive(true);
        if (tichu == Tichu.smallTichu) smallTichuLogo.SetActive(true);
    }
    #endregion
    public void SetupDragon(int score)
    {
        ResetPannel(false);
        if (submitManager.GetSubmitPlayer() == PlayerManager.GetPlayerOwn())
        {
            ActiveSendDragonArea(true);
            this.score = score;
        }
    }
    public void SendDragon(int index)//1 : ¿ÞÂÊ 3 : ¿À¸¥ÂÊ
    {
        int receivePlayer = PlayerManager.GetPlayerWithDirection(index).Index;
        gameManager.TrySendDragon(receivePlayer, score);
    }
}
