using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDRound : MonoBehaviour
{
    [SerializeField] Lantern enemy;
    [SerializeField] Lantern lantern;
    [SerializeField] Lantern player;
    public void SetRound(List<NDSubmit> submits, Winner winner, bool isAboutVicory)
    {
        foreach(NDSubmit submit in submits)
        {
            if (submit.isMine) player.SetImage(CardImageConverter.instance.GetSprite(submit.rank, submit.team.ToString()));
            else
            {
                int rank = !isAboutVicory ? EvenConverter.GetIndexByIsEven(submit.rank) : submit.rank;
                enemy.SetImage(CardImageConverter.instance.GetSprite(rank, submit.team.ToString()));//°¡¸®±â
            }
        }
        lantern.SetColor(ColorSelector.CardColor(winner.ToString()));
    }
}
