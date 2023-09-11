using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwapManager : MonoBehaviour
{
    [SerializeField] TichuGameManager gameManager;
    [SerializeField] SwapPannelPopup popup;
    [SerializeField] CardChecker cardChecker;
    [SerializeField] List<CardDisplayer> cardDisplayers = new List<CardDisplayer>();
    [SerializeField] PannelManager pannelManager;
    public void OpenPopup()
    {
        TichuPlayer player = (TichuPlayer)TichuPlayerManager.GetPlayerOwn();
        popup.Setup(player.RecieveCard);
    }
    public bool IsAllSwap()
    {
        foreach(TichuPlayer player in TichuPlayerManager.GetAllPlayerWithTurn())
        {
            if (!player.IsAllSwap()) return false;
        }
        return true;
    }
    public void TogglePannel(bool isOn)
    {
        foreach (CardDisplayer displayer in cardDisplayers)
        {
            displayer.SetSendRecieve(isOn);
            displayer.SetCard();
        }
        if (!isOn) cardChecker.CardSetup();
    }
    public void SwapAllComplete()
    {
        cardChecker.CardSetup();
    }
    public void SwapCard(int index)
    {
        if (cardChecker.SelectedCards.Count != 1) return;
        List<CardInfo> sendCard = cardChecker.SelectedCards;
        TichuPlayer player;
        player = (TichuPlayer)TichuPlayerManager.GetPlayerOwn();
        player.TryUseCardWithSwap(sendCard);
        player = (TichuPlayer)TichuPlayerManager.GetPlayerWithDirection(index);
        player.TryReceivieCard(sendCard, TichuPlayerManager.GetPlayerOwn().Index);
        cardChecker.HideCard();
        foreach (CardDisplayer displayer in cardDisplayers)
        {
            displayer.SetSendRecieve(true);
        }
        pannelManager.ActiveSendBtn(false, index - 1);
    }
}
