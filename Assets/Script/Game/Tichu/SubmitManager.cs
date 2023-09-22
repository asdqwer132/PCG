using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SubmitManager : MonoBehaviour
{
    [SerializeField] CardListDisplayer cardList;
    Genealogy submitedGenealogy = new Genealogy(0, 0, GenealogyType.none);
    Player submitPlayer;
    [SerializeField] int score;//
    bool isDragon;
    public bool IsDragon() { return isDragon; }
    public int GetScore() { return score; }
    public Genealogy GetGenealogy() { return submitedGenealogy; }
    public Player GetSubmitPlayer() { return submitPlayer; }
    public bool IsFirst() { return submitedGenealogy.length == 0; }
    public void Submit(List<CardInfo> selectedCards, int index)
    {
        submitPlayer = PlayerManager.GetAllPlayerWithTurn()[index];
        Genealogy genealogy = GenealogyChekcer.Instance.CheckGenealogy(selectedCards);
        if(genealogy.genealogyType == GenealogyType.single && genealogy.rank == (int)Animal.phoenix)//피닉스
        {
            genealogy.rank = submitedGenealogy.rank + 0.5f;
        }
        submitedGenealogy = genealogy;
        if (submitedGenealogy.genealogyType == GenealogyType.single && submitedGenealogy.rank == 15 || isDragon) isDragon = true;
        score += ScoreCalculater.CalculateScore(selectedCards);
        cardList.SetCard(selectedCards);
    }
    public void ResetAll()
    {
        submitedGenealogy = new Genealogy(0, 0, GenealogyType.none);
        score = 0;
        isDragon = false;
        cardList.ResetActive();
    }
    public bool IsBoom(List<CardInfo> cardInfos)
    {
        if (AnimalChecker.CheckAnimal(Animal.phoenix, cardInfos)) return false;
        Genealogy newGenealogy = GenealogyChekcer.Instance.CheckGenealogy(cardInfos);
        if (newGenealogy.genealogyType == GenealogyType.fourOfKind)//바크탄
        {
            return submitedGenealogy.genealogyType != GenealogyType.straightFlush;
        }
        if (newGenealogy.genealogyType == GenealogyType.straightFlush) { return true; }//스티플바크탄
        return false;
    }
    public bool CheckCanSubmit(Genealogy newGenealogy)
    {
        if (newGenealogy.rank == (int)Animal.phoenix && submitedGenealogy.genealogyType == GenealogyType.single) return true;
        if (submitedGenealogy.length == 0 && newGenealogy.length != 0) return true;
        if (submitedGenealogy.genealogyType == newGenealogy.genealogyType && submitedGenealogy.rank < newGenealogy.rank && submitedGenealogy.length == newGenealogy.length) return true;
        return false;
    }
}
