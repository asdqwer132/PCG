using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public struct Genealogy
{
    public float rank;
    public int length;
    public GenealogyType genealogyType;

    public Genealogy(int rank, int length, GenealogyType genealogyType)
    {
        this.rank = rank;
        this.length = length;
        this.genealogyType = genealogyType;
    }
}
public class GenealogyChekcer
{
    public static bool CheckMakeGenealogy(int callRank, List<CardInfo> cardInfos, Genealogy genealogy)
    {
        if (genealogy.genealogyType == GenealogyType.single) return CanMakeSingle(callRank, cardInfos);
        if (genealogy.genealogyType == GenealogyType.straight) return CanMakeStraight(callRank, genealogy.length, cardInfos);
        return false;
    }
    public static bool IsBomb(List<CardInfo> selectedCard)
    {
        Genealogy genealogy = CheckGenealogy(selectedCard);
        return genealogy.genealogyType == GenealogyType.fourOfKind || genealogy.genealogyType == GenealogyType.straightFlush;
    }
    public static Genealogy CheckGenealogy(List<CardInfo> selectedCard)
    {
        if(selectedCard.Count > 0)
        {
            bool IsPhoanix = AnimalChecker.CheckAnimal(Animal.phoenix, selectedCard);
            int index = selectedCard.FindIndex(x => x.cardType == Animal.phoenix.ToString());
            int roop = IsPhoanix ? 14 : 2;
            for(int i = roop + 1; i > 1; i--)
            {
                List<CardInfo> cards = new List<CardInfo>();
                cards = selectedCard.ToList();
                if (isSIngle(cards)) return new Genealogy(GetRank(cards), cards.Count, GenealogyType.single);
                if (IsPhoanix)
                {
                    cards[index] = new CardInfo("null", i);
                    cards.Sort((x, y) => x.number.CompareTo(y.number));
                }
                if (CanGenealogy(cards))
                {
                    if (isPair(cards)) return new Genealogy(GetRank(cards), cards.Count, GenealogyType.pair);
                    if (isStraightPair(cards)) return new Genealogy(GetRank(cards), cards.Count, GenealogyType.straightPair);
                    if (isTrple(cards)) return new Genealogy(GetRank(cards), cards.Count, GenealogyType.triple);
                    if (isStraightTriple(cards)) return new Genealogy(GetRank(cards), cards.Count, GenealogyType.straightTriple);
                    if (isFullHouse(cards)) return new Genealogy(GetRank(cards), cards.Count, GenealogyType.fullHouse);
                    if (isStraight(cards))
                    {
                        if (isStraightFlush(cards)) return new Genealogy(GetRank(cards), cards.Count, GenealogyType.straightFlush);
                        return new Genealogy(GetRank(cards), cards.Count, GenealogyType.straight);
                    }
                    if (isFourOfKind(cards)) return new Genealogy(GetRank(cards), cards.Count, GenealogyType.fourOfKind);
                }
            }
        }
        return new Genealogy(0, 0, GenealogyType.none); ;

        bool CanGenealogy(List<CardInfo> cards)
        {
            foreach (CardInfo cardInfo in cards)
            {
                if (cardInfo.number == -1 || cardInfo.number == 15) return false;
            }
            return true;
        }
    }
    static int GetRank(List<CardInfo> cards)
    {
        if (isFullHouse(cards))
        {
            Dictionary<int, int> rankCount = new Dictionary<int, int>();

            // 카드 랭크 빈도수 카운트
            foreach (CardInfo card in cards)
            {
                if (rankCount.ContainsKey(card.number))
                    rankCount[card.number]++;
                else
                    rankCount[card.number] = 1;
            }
            foreach (int rank in rankCount.Keys)
            {
                if (rankCount[rank] == 3) return rank;
            }
        }
        return cards[cards.Count - 1].number;
    }
    #region 카드족보 확인코드
    static bool isSIngle(List<CardInfo> cards) { return cards.Count == 1; }
    static bool isPair(List<CardInfo> cards)
    {
        if (cards.Count != 2) return false;
        if (cards[0].number != cards[cards.Count - 1].number) return false;
        return true;
    }
    static bool isTrple(List<CardInfo> cards)
    {
        if (cards.Count != 3) return false;
        if (cards[0].number != cards[cards.Count - 1].number) return false;
        return true;
    }
    static bool isFullHouse(List<CardInfo> cards)
    {
        if (cards.Count != 5) return false;
        Dictionary<int, int> rankCount = new Dictionary<int, int>();

        // 카드 랭크 빈도수 카운트
        foreach (CardInfo card in cards)
        {
            if (rankCount.ContainsKey(card.number))
                rankCount[card.number]++;
            else
                rankCount[card.number] = 1;
        }

        bool hasThreeOfAKind = false;
        bool hasPair = false;

        // 풀하우스 조건 검사
        foreach (int rank in rankCount.Keys)
        {
            if (rankCount[rank] == 3)
                hasThreeOfAKind = true;
            else if (rankCount[rank] == 2)
                hasPair = true;
        }

        return hasThreeOfAKind && hasPair;
    }
    static bool isFourOfKind(List<CardInfo> cards)
    {
        if (cards.Count != 4) return false;
        if (cards[0].number != cards[cards.Count - 1].number) return false;
        return true;
    }
    static bool isStraight(List<CardInfo> cards)
    {
        if (cards.Count < 5) return false;
        for(int i = 1; i < cards.Count; i++)
        {
            if (cards[i].number != cards[i - 1].number + 1) return false;
        }
        return true;
    }
    static bool isStraightFlush(List<CardInfo> cards)
    {
        if (!isStraight(cards)) return false;
        string standard = cards[0].cardType;
        foreach (CardInfo card in cards)
        {
            if (card.cardType != standard) return false;
        }
        return true;
    }
    static bool isStraightPair(List<CardInfo> cards)
    {
        if (cards.Count % 2 != 0 || cards.Count < 4) return false;
        if (cards[0].number != cards[1].number) return false;
        for (int i = 2; i < cards.Count; i += 2)
        {
            if (cards[i].number != cards[i - 2].number + 1) return false;
        }
        for (int i = 3; i < cards.Count; i += 2)
        {
            if (cards[i].number != cards[i - 2].number + 1) return false;
        }
        return true;
    }
    static bool isStraightTriple(List<CardInfo> cards)
    {
        if (!TichuModeManager.GetStMode()) return false;
        if (cards.Count % 3 != 0 || cards.Count < 6) return false;
        if (!(cards[0].number == cards[1].number && cards[0].number == cards[2].number)) return false;
        for (int i = 3; i < cards.Count; i += 3)
        {
            if (cards[i].number != cards[i - 3].number + 1) return false;
        }
        for (int i = 4; i < cards.Count; i += 3)
        {
            if (cards[i].number != cards[i - 3].number + 1) return false;
        }
        for (int i = 5; i < cards.Count; i += 3)
        {
            if (cards[i].number != cards[i - 3].number + 1) return false;
        }
        return true;
    }
    #endregion
    #region 카드족보 생성가능 확인코드
    public static List<CardInfo> MakedStraight(int callRank, int length, List<CardInfo> cardInfos)
    {
        if (cardInfos.Count < length) return null;
        List<CardInfo> deduplicationCardInfos = new List<CardInfo>();
        foreach (CardInfo cardInfo in cardInfos)
        {
            if (deduplicationCardInfos.Find(x => x.number == cardInfo.number) == null) deduplicationCardInfos.Add(cardInfo);
        }
        for (int i = length; i <= deduplicationCardInfos.Count; i++)
        {
            List<CardInfo> checkList = new List<CardInfo>();
            for (int index = 0; index < length; index++)
            {
                Debug.Log(i + "i/c" + index + (i - length) + "/index" + index);
                checkList.Add(deduplicationCardInfos[index + (i - length)]);
            }
            if (isStraight(checkList) && checkList.Find(x => x.number == callRank) != null) return checkList;
        }
        return null;
    }
    public static List<CardInfo> MakedSingle(int callRank, List<CardInfo> cardInfos)
    {
        foreach (CardInfo cardInfo in cardInfos)
        {
            if (cardInfo.number == callRank) return new List<CardInfo>() { cardInfo };
        }
        return null;
    }
    public static bool CanMake4k(int callRank, List<CardInfo> cardInfos)
    {
        Dictionary<int, int> rankCount = new Dictionary<int, int>();

        // 카드 랭크 빈도수 카운트
        foreach (CardInfo card in cardInfos)
        {
            if (rankCount.ContainsKey(card.number))
                rankCount[card.number]++;
            else
                rankCount[card.number] = 1;
        }
        if(rankCount.ContainsKey(callRank)) return rankCount[callRank] == 4;
        return false;
    }
    public static bool CanMakeSF(int callRank, List<CardInfo> handCard)
    {
        List<CardInfo> calledCards = new List<CardInfo>();
        foreach(CardInfo cardInfo in handCard)
        {
            if (cardInfo.number == callRank) calledCards.Add(cardInfo);//선언된 카드 수집
        }
        foreach (CardInfo calledCard in calledCards)//카드의 문양별로 정렬
        {
            List<CardInfo> cardListSortedByColor = new List<CardInfo>();
            foreach(CardInfo cardInfo in handCard)
            {
                if (cardInfo.cardType == calledCard.cardType) cardListSortedByColor.Add(cardInfo);
            }
            //모든 문양별 카드를 확인
            if (CanMakeStraight(callRank, 5, cardListSortedByColor)) return true;
        }
        return false;
    }
    static bool CanMakeSingle(int callRank, List<CardInfo> cardInfos)
    {
        foreach (CardInfo cardInfo in cardInfos)
        {
            if (cardInfo.number == callRank) return true;
        }
        return false;
    }
    static bool CanMakeStraight(int callRank, int length, List<CardInfo> cardInfos)
    {
        if (cardInfos.Count < length) return false;
        List<CardInfo> deduplicationCardInfos = new List<CardInfo>();
        foreach (CardInfo cardInfo in cardInfos)
        {
            if (deduplicationCardInfos.Find(x => x.number == cardInfo.number) == null) deduplicationCardInfos.Add(cardInfo);
        }
        for (int i = length; i <= deduplicationCardInfos.Count; i++)
        {
            List<CardInfo> checkList = new List<CardInfo>();
            for (int index = 0; index < length; index++)
            {
                checkList.Add(deduplicationCardInfos[index + (i - length)]);
            }
            if (isStraight(checkList) && checkList.Find(x => x.number == callRank) != null) return true;
        }
        return false;
    }
    #endregion
}
