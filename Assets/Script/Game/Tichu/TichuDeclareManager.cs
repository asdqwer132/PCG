using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TichuDeclareManager : MonoBehaviour
{
    [SerializeField] PannelManager pannelManager;
    [SerializeField] Image background;
    [SerializeField] List<CardDisplayer> cardDisplayers = new List<CardDisplayer>();
    Tichu currentMaxTichu;
    public bool HasTichu(Tichu tichu)
    {
        foreach (TichuPlayer player in PlayerManager.GetAllPlayerWithTurn()) { if (player.Tichu == tichu) return true; }
        return false;
    }
    public bool IsAllDecideTichu()
    {
        foreach(TichuPlayer player in PlayerManager.GetAllPlayerWithTurn()) { if (player.Tichu == Tichu.none) return false; }
        return true;
    }
    public void ResetAll()
    {
        foreach (TichuPlayer player in PlayerManager.GetAllPlayerWithTurn()) {  player.TrySetTichu(Tichu.none); }
        foreach (CardDisplayer cardDisplayer in cardDisplayers) { cardDisplayer.SetTichu(); }
        SetBackground(Tichu.none);
    }
    public void NoTichu()
    {
        TichuPlayer player = (TichuPlayer)PlayerManager.GetPlayerOwn();
        player.TrySetTichu(Tichu.noTichu);
        pannelManager.ActiveLargeTichuArea(false);
    }
    public void LargeTichu()
    {
        TichuPlayer player = (TichuPlayer)PlayerManager.GetPlayerOwn();
        player.TrySetTichu(Tichu.largeTichu);
        pannelManager.ActiveLargeTichuArea(false);
        pannelManager.ActiveTichuLogo(Tichu.largeTichu);
    }
    public void SmallTichu()
    {
        TichuPlayer player = (TichuPlayer)PlayerManager.GetPlayerOwn();
        player.TrySetTichu(Tichu.smallTichu);
        pannelManager.ActiveSmallTichuArea(false);
    }
    public void SetBackground(Tichu tichu)
    {
        if (tichu == Tichu.none)
        {
            currentMaxTichu = Tichu.none;
            background.color = ColorSelector.BackgroundColor(tichu);
            return;
        }
        if (tichu == Tichu.largeTichu)
        {
            background.color = ColorSelector.BackgroundColor(tichu); 
            currentMaxTichu = Tichu.largeTichu;
            PlayMusicOperator.Instance.PlayBGM("largeTichu");
        }
        if (tichu == Tichu.smallTichu && currentMaxTichu != Tichu.largeTichu)
        {
            background.color = ColorSelector.BackgroundColor(tichu);
            PlayMusicOperator.Instance.PlayBGM("smallTichu");
        }
    }
}
