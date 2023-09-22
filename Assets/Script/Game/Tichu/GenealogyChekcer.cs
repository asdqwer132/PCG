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
public class GenealogyChekcer : MonoBehaviour
{
    #region 싱글퇀
    static GenealogyChekcer instance;
    public static GenealogyChekcer Instance { get => instance; set => instance = value; }
    private void OnDestroy()
    {
        instance = null;
    }
    private void Start()
    {
        if (instance == null) instance = this;
    }
    #endregion
    #region 사전 관리
    Dictionary<int, int> CardCount = new Dictionary<int, int>()
    {
        {-1,0 },
        {0,0 },
        {1,0 },
        {2,0 },
        {3,0 },
        {4,0 },
        {5,0 },
        {6,0 },
        {7,0 },
        {8,0 },
        {9,0 },
        {10,0 },
        {11,0 },//j
        {12,0 },//q
        {13,0 },//k
        {14,0 },//a
        {15,0 }//용
    };
    void SetCardCount(List<CardInfo> cardInfos)
    {
        ResetCardCount();
        for (int i = 0; i < cardInfos.Count; i++)
        {
            if(cardInfos[i].number != (int)Animal.phoenix) CardCount[cardInfos[i].number]++;
        }
    }
    void ResetCardCount()
    {
        for (int i = -1; i <= 15; i++)
        {
            CardCount[i] = 0;
        }
    }
    List<int> GetRefinedNum()
    {
        List<int> newList = new List<int>();
        for (int i = 0; i <= 15; i++)
        {
            if (CardCount[i] > 0) newList.Add(i);
        }
        return newList;
    }
    #endregion
    public bool IsBomb(List<CardInfo> selectedCard)
    {
        SetCardCount(selectedCard);
        List<int> refinedCard = GetRefinedNum();
        bool IsPhoanix = AnimalChecker.CheckAnimal(Animal.phoenix, selectedCard);
        if ((IsStraight(refinedCard, 5) && IsFlush(selectedCard) || IsPair(refinedCard, 4)) && !IsPhoanix) return true;
        return false;
    }
    public bool CanMake(List<CardInfo> selectedCard, Genealogy genealogy, int callRank)
    {
        if (callRank <= genealogy.rank) return false;
        bool IsPhoanix = AnimalChecker.CheckAnimal(Animal.phoenix, selectedCard);
        int roop = IsPhoanix ? 14 : 2;
        for (int i = roop; i >= 2; i--)
        {
            SetCardCount(selectedCard);
            if (CardCount[callRank] == 0) return false;
            if (CardCount[callRank] == 4) return true;//폭탄
            if (IsPhoanix) CardCount[i]++;
            switch (genealogy.genealogyType)//단순 개수 계열
            {
                case GenealogyType.none:
                    return true;
                case GenealogyType.single:
                    return CardCount[callRank] >= 1;
                case GenealogyType.pair:
                    return CardCount[callRank] >= 2;
                case GenealogyType.triple:
                    return CardCount[callRank] >= 3;
                case GenealogyType.fourOfKind:
                    return CardCount[callRank] >= 4;
                case GenealogyType.fullHouse:
                    return CanMakeFullHouse();
            }
            List<int> refinedCard = GetRefinedNum();
            int length = 0;
            switch (genealogy.genealogyType)//스트레이트 계?열
            {
                case GenealogyType.straight:
                    length = genealogy.length;
                    Debug.Log("스트레이트? 길이 / 랭크" + length + "/" + callRank);
                    return CanMakeStraight(refinedCard, length, 1);
                case GenealogyType.straightPair:
                    length = genealogy.length / 2;
                    return CanMakeStraight(refinedCard, length, 2);
                case GenealogyType.straightTriple:
                    length = genealogy.length / 3;
                    return CanMakeStraight(refinedCard, length, 3);
                case GenealogyType.straightFlush:
                    length = genealogy.length;
                    List<string> standards = new List<string>();
                    foreach(CardInfo cardInfo in selectedCard)
                    {
                        if (cardInfo.number == callRank) standards.Add(cardInfo.cardType);
                    }
                    foreach(string standard in standards)
                    {
                        List<CardInfo> cardInfos = new List<CardInfo>();
                        foreach (CardInfo cardInfo in selectedCard)
                        {
                            if (cardInfo.cardType == standard) cardInfos.Add(cardInfo);
                        }
                        SetCardCount(cardInfos);
                        refinedCard = GetRefinedNum();
                        if (CanMakeStraight(refinedCard, length, 1)) return true;
                    }
                    return false;
            }
        }
        return false;
        bool CanMakeFullHouse()
        {
            int pair = 0, triple = 0;
            if (CardCount[callRank] < 2) return false;
            for(int i = 2; i <= 14;i++)
            {
                if (CardCount[i] >= 2) pair++;
                if (CardCount[i] >= 3) triple++;
            }
            if (triple >= 1 && pair >= 2) return true;
            return false;
        }
        bool CanMakeStraight(List<int> refinedCard, int length, int count)
        {
            foreach(int ran in refinedCard)
            {
                Debug.Log("rank = " + ran);
            }
            Debug.Log("for = " + (refinedCard.Count - length + 1));
            if (refinedCard.Count < length) return false;
            for(int i =0;i< refinedCard.Count - length + 1; i++)
            {
                List<int> refindedRefinedCard = new List<int>();
                for(int rank = i; rank < i + length; rank++)
                {
                    if (CardCount[refinedCard[rank]] < count) return false;
                    refindedRefinedCard.Add(refinedCard[rank]);
                }
                foreach (int ran in refindedRefinedCard)
                {
                    Debug.Log("rankrank = " + ran);
                }
                if (IsStraight(refindedRefinedCard, length))
                {
                    Debug.Log("스투뤠잇");
                    if (refindedRefinedCard.FindIndex(x => x == callRank) >= 0) return true;
                }
            }
            return false;
        }
    }
    public Genealogy CheckGenealogy(List<CardInfo> selectedCard)
    {
        if (selectedCard.Count > 0)
        {
            bool IsPhoanix = AnimalChecker.CheckAnimal(Animal.phoenix, selectedCard);
            int roop = IsPhoanix ? 14 : 2;
            for (int i = roop; i >= 2; i--)
            {
                SetCardCount(selectedCard);
                if (IsPhoanix) CardCount[i]++;
                List<int> refinedCard = GetRefinedNum();
                if (IsPair(refinedCard, 1))
                {
                    int rank = IsPhoanix ? (int)Animal.phoenix : GetRank(false);
                    return new Genealogy(rank, selectedCard.Count, GenealogyType.single);
                }//싱글
                if (CanGenealogy())
                {
                    if (IsPair(refinedCard, 2)) { return new Genealogy(GetRank(false), selectedCard.Count, GenealogyType.pair); }//페어
                    if (IsPair(refinedCard, 3)) { return new Genealogy(GetRank(false), selectedCard.Count, GenealogyType.triple); }//트리플
                    if (IsPair(refinedCard, 4) && !IsPhoanix) { return new Genealogy(GetRank(false), selectedCard.Count, GenealogyType.fourOfKind); }//포카드
                    if (IsFullHouse(refinedCard)) { return new Genealogy(GetRank(true), selectedCard.Count, GenealogyType.fullHouse); }//풀하우스
                    if (isStraightPair(refinedCard, 2)) { return new Genealogy(GetRank(false), selectedCard.Count, GenealogyType.straightPair); }//스트레이트 페어
                    if (isStraightPair(refinedCard, 3)) { return new Genealogy(GetRank(false), selectedCard.Count, GenealogyType.straightTriple); }//스트레이트 트리플
                    if (IsStraight(refinedCard, 5))//스트레이트
                    {
                        if (IsFlush(selectedCard)) { return new Genealogy(GetRank(false), selectedCard.Count, GenealogyType.straightFlush); }//스트레이트 플러시
                        return new Genealogy(GetRank(false), selectedCard.Count, GenealogyType.straight);
                    }
                }
            }
        }
        return new Genealogy(0, 0, GenealogyType.none);
        bool CanGenealogy()
        {
            if (CardCount[-1] > 0 || CardCount[15] > 0) return false; 
            return true;
        }
    }
    #region 카드족보 확인코드
    int GetRank(bool isFullHouse)
    {
        int maxRankCount = isFullHouse ? 3 : 1;
        for (int i = 15; i >= 2; i--)
        {
            if (CardCount[i] >= maxRankCount) return i;
        }
        return -1;
    }
    bool IsFlush(List<CardInfo> cardInfos)
    {
        string standard = cardInfos[0].cardType;
        foreach(CardInfo card in cardInfos)
        {
            if (card.cardType != standard) return false;
        }
        return true;
    }
    bool IsFullHouse(List<int> cards)
    {
        if (cards.Count != 2) return false;
        if (CardCount[cards[0]] == 3 && CardCount[cards[1]] == 2) return true;
        if (CardCount[cards[1]] == 3 && CardCount[cards[0]] == 2) return true;
        return false;
    }
    bool IsPair(List<int> cards, int length)
    {
        if (cards.Count < 1) return false;
        if (CardCount[cards[0]] != length) return false;
        return cards[0] == cards[cards.Count - 1];
    }
    bool IsStraight(List<int> cards, int minLength)
    {
        if (cards.Count < minLength) return false;
        return cards.Count - (cards[cards.Count - 1] - cards[0]) == 1;
    }
    bool IsStraight(List<int> cards)
    {
        if (cards.Count < 2) return false;
        return cards.Count - (cards[cards.Count - 1] - cards[0]) == 1;
    }
    bool isStraightPair(List<int> cards, int width)
    {
        if (!IsStraight(cards)) return false;
        foreach(int rank in cards)
        {
            if (CardCount[rank] != width) return false;
        }
        return true;
    }
    #endregion
}
