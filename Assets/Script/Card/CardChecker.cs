using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardChecker : MonoBehaviour
{
    [SerializeField] SubmitManager submitManager;
    [SerializeField] Submit submit;
    [SerializeField] CardListDisplayer cardList;
    [SerializeField] List<CardInfo> selectedCards = new List<CardInfo>();//
    [SerializeField] Player player;//
    public List<CardInfo> SelectedCards { get => selectedCards; }
    public bool IsDog() { return selectedCards[0].number == 0; }
    public bool IsSelected() { return selectedCards.Count > 0; }
    public void Setup(Player player) { this.player = player; }
    public void CardSetup()
    {
        cardList.SetCard(player.HandCards);
        //for (int i = 0; i < player.HandCards.Count; i++)
        //{
        //    cards[i].gameObject.SetActive(true);
        //    cards[i].Setup(player.HandCards[i]);
        //}
    }
    public void ResetCards()
    {
        cardList.ResetAll();
        //foreach (Card card in cards)
        //{
        //    card.gameObject.SetActive(false);
        //    card.gameObject.GetComponent<CardSelector>().IsSelectced = false;
        //}
        selectedCards = new List<CardInfo>();
    }
    public void HideCard()
    {
        foreach (CardInfo card in selectedCards)
        {
            cardList.HideCard(card);
        }
        selectedCards = new List<CardInfo>(); 
        SetText();
    }
    public void SelectCard(CardInfo cardInfo)
    {
        selectedCards.Add(cardInfo);
        selectedCards.Sort((x, y) => x.number.CompareTo(y.number));
        SetText();
    }
    public void DeleteCard(CardInfo cardInfo)
    {
        selectedCards.Remove(cardInfo); 
        SetText();
    }
    void SetText()
    {
        Genealogy genealogy = GenealogyChekcer.CheckGenealogy(selectedCards);
        submit.SetTextByGenealogy(genealogy, submitManager.CheckCanSubmit(genealogy));
        if (submitManager.IsBoom(genealogy)) submit.ForceSetBtn();
    }
}
