using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    public static Color TeamColor(string team)
    {
        if (team == Team.blue.ToString()) return new Color(0.5f, 0.5f, 1f);
        if (team == Team.red.ToString()) return new Color(1f, 0.5f, 0.5f);
        return Color.white;
    }
    public static Color OnOffColor(bool isOn)
    {
        if (isOn) return Color.yellow;
        return Color.white;
    }
    public static Color CardColor(string cardType)
    {
        if (cardType == CardType.Red.ToString()) return Color.red;
        if (cardType == CardType.Yellow.ToString()) return Color.yellow;
        if (cardType == CardType.Blue.ToString()) return Color.blue;
        if (cardType == CardType.Green.ToString()) return Color.green;
        return Color.black;
    }
    public static Color BackgroundColor(Tichu tichu)
    {
        if (tichu == Tichu.largeTichu) return new Color(1f, 0.5f, 0.5f);
        if (tichu == Tichu.smallTichu) return new Color(0.5f, 0.5f, 1f);
        return new Color(0.7f, 1f, 0.7f);
    }
}
