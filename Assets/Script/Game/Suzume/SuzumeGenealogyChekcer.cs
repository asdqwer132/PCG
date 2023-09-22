using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzumeGenealogyChekcer : MonoBehaviour
{
    private void Start()
    {
        List<CardInfo> cardInfos = new List<CardInfo>();
        //Green
        //Red
        cardInfos.Add(new CardInfo("Green", 1));
        cardInfos.Add(new CardInfo("Green", 2));
        cardInfos.Add(new CardInfo("Green",  3));
        cardInfos.Add(new CardInfo("Green",  6));
        cardInfos.Add(new CardInfo("Red",  7));
        cardInfos.Add(new CardInfo("Green", 8));
        Debug.Log(CheckGenealogy(cardInfos, new CardInfo("dora", 1)));
    }
    public int CheckGenealogy(List<CardInfo> playerCard, CardInfo dora)// 6��
    {
        List<List<CardInfo>> separatedBody = IsComplete(playerCard);
        if (separatedBody.Count < 2) return 0;//ȭ�� �Ұ�
        int score = 0;
        //����
        score = AllGreen(playerCard);
        if (score == 10) return score;
        score = Chingyao(playerCard);
        if (score == 15) return score;
        score = AllRed(playerCard);
        if (score == 20) return score;

        //���ʽ�
        foreach (List<CardInfo> cardInfos in separatedBody)
        {
          //  Debug.Log("-----------------");
            //foreach(CardInfo card in cardInfos)
            //{
            //    Debug.Log(card.number + "/" + card.cardType);
            //}
            score += IsStraightByCard(cardInfos);
            score += isTripleByCard(cardInfos);
        }
      //  Debug.Log("�Ϲ� ���� " + score);
        score += Red(playerCard);
        score += Dora(playerCard, dora);
        score += Tanyao(playerCard);
        score += Chanta(separatedBody[0], separatedBody[1]);
        return score;
    }
    #region ����
    List<List<CardInfo>> IsComplete(List<CardInfo> playerCard)
    {
        List<List<CardInfo>> separatedbody = new List<List<CardInfo>>();
        List<SuzumeGenealogy> genealogy = new List<SuzumeGenealogy>();
        Dictionary<int, int> rankCount = new Dictionary<int, int>();
        List<int> rankList = new List<int>();
        // ī�� ��ũ �󵵼� ī��Ʈ
        foreach (CardInfo card in playerCard)
        {
            if (rankCount.ContainsKey(card.number))
                rankCount[card.number]++;
            else
                rankCount[card.number] = 1;
        }
        foreach (int rank in rankCount.Keys)
        {
            if (rankCount[rank] > 0)
            {
                rankList.Add(rank);
            }
        }
        //Ʈ���� �˻�
        for (int i = 0; i < rankList.Count; i++)
        {
            if (rankCount[rankList[i]] >= 3)
            {
                genealogy.Add(SuzumeGenealogy.triple);
                List<CardInfo> body = new List<CardInfo>();
                foreach (CardInfo card in playerCard)
                {
                    if (card.number == rankList[i]) body.Add(card);
                }
                separatedbody.Add(body);
                rankCount[rankList[i]] -= 3;
            }
        }
        //��Ʈ����Ʈ �˻�
        List<int> body1Num = new List<int>();
        List<int> body2Num = new List<int>();
        int count = 0;
        for (int i = 0; i < rankList.Count; i++)
        {
            if (rankCount[rankList[i]] > 0)
            {
                body1Num.Add(rankList[i]);
                rankCount[rankList[i]]--;
                count++;
                if (count >= 3) break;
            }
        }
        count = 0;
        for (int i = 0;i < rankList.Count; i++)
        {
            if (rankCount[rankList[i]] > 0)
            {
                body2Num.Add(rankList[i]);
                rankCount[rankList[i]]--;
                count++;
                if (count >= 3) break;
            }
        }
        //Debug.Log("1-------------");
        //foreach (int num in body1Num)
        //{
        //    Debug.Log(num);
        //}
        //Debug.Log("2-------------");
        //foreach (int num in body2Num)
        //{
        //    Debug.Log(num);
        //}

        if (IsStraight(body1Num))
        {
            List<CardInfo> body = new List<CardInfo>();
            foreach(int num in body1Num)
            {
                foreach (CardInfo card in playerCard)
                {
                    if (card.number == num)
                    {
                        body.Add(card);
                        break;
                    }
                }
            }
            separatedbody.Add(body);
            genealogy.Add(SuzumeGenealogy.straight);
        }
        if (IsStraight(body2Num))
        {
            List<CardInfo> body = new List<CardInfo>();
            foreach (int num in body2Num)
            {
                foreach (CardInfo card in playerCard)
                {
                    if (card.number == num)
                    {
                        body.Add(card);
                        break;
                    }
                }
            }
            separatedbody.Add(body);
            genealogy.Add(SuzumeGenealogy.straight);
        }
        return separatedbody;
    }
    bool IsStraight(List<int> cardInfos)
    {
        if (cardInfos.Count < 3) return false;
        for (int i = 0; i < cardInfos.Count; i++)
        {
            if (cardInfos[i] == 10) return false;
        }
        for (int i = 1; i < cardInfos.Count; i++)
        {
            if (cardInfos[i - 1] != cardInfos[i] - 1) return false;
        }
        return true;
    }
    #endregion
    #region ���ʽ� ��� �Լ�
    int IsStraightByCard(List<CardInfo> cardInfos)
    {
        List<int> num = new List<int>();
        foreach(CardInfo cardInfo in cardInfos)
        {
            num.Add(cardInfo.number);
        }
        if (IsStraight(num)) return 1;
        return 0;
    }
    int isTripleByCard(List<CardInfo> cardInfos)
    {
        for (int i = 1; i < cardInfos.Count; i++)
        {
            if (cardInfos[i - 1].number != cardInfos[i].number) return 0;
        }
        return 2;
    }
    int Red(List<CardInfo> cardInfos)//������
    {
        int bonus = 0;
        foreach (CardInfo cardInfo in cardInfos)
        {
            if (cardInfo.cardType == CardType.Red.ToString())
            {
                bonus++;
                Debug.Log("������");
            }
        }
        return bonus;
    }
    int Dora(List<CardInfo> cardInfos, CardInfo dora)//����
    {
        int bonus = 0;

        foreach (CardInfo cardInfo in cardInfos)
        {
            if (cardInfo.cardType == dora.cardType && cardInfo.number == dora.number)
            {
                bonus++;
                Debug.Log("����");
            }
        }
        return bonus;
    }
    int Tanyao(List<CardInfo> cardInfos)//����
    {
        foreach(CardInfo cardInfo in cardInfos)
        {
            if (!IsTangyaoNum(cardInfo)) return 0;
        }
        Debug.Log("���߿�");
        return 1;
    }
    int Chanta(List<CardInfo> body1, List<CardInfo> body2)//îŸ
    {
        bool isChanta1 = false, isChanta2 = false;
        foreach (CardInfo cardInfo in body1)
        {
            Debug.Log("body1" + cardInfo.number);
            if (IsChantaNum(cardInfo))
            {
                isChanta1 = true;
                //break;
            }
        }
        foreach (CardInfo cardInfo in body2)
        {
            Debug.Log("body2" + cardInfo.number);
            if (IsChantaNum(cardInfo))
            {
                isChanta2 = true;
                //break;
            }
        }
        if (isChanta1 && isChanta2)
        {
            Debug.Log("��Ÿ");
            return 2;
        }
        return 0;
    }
    #endregion
    #region ���� ��� �Լ�
    int AllGreen(List<CardInfo> cardInfos)//�ñ׸�
    {
        foreach (CardInfo cardInfo in cardInfos)
        {
            if (cardInfo.cardType != CardType.Green.ToString()) return 0;//�����϶�
            else//�׸��̱� �ѵ�
            {
                if (IsGreenCardButRed(cardInfo.number)) return 0;//���尡 �������� ��
            }
        }
        return 10;
    }
    int AllRed(List<CardInfo> cardInfos)//�÷���
    {
        foreach (CardInfo cardInfo in cardInfos)
        {
            if (cardInfo.cardType != CardType.Red.ToString()) return 0;
        }
        return 20;
    }
    int Chingyao(List<CardInfo> cardInfos)//Ī�߿�
    {
        foreach (CardInfo cardInfo in cardInfos)
        {
            if (!IsChantaNum(cardInfo)) return 0;
        }
        return 15;
    }
    #endregion
    bool IsTangyaoNum(CardInfo cardInfo) { return cardInfo.number >= 2 && cardInfo.number <= 8; }
    bool IsChantaNum(CardInfo cardInfo) { return !IsTangyaoNum(cardInfo); }



    bool IsGreenCardButRed(int rank)
    {
        if (rank == 1 || rank == 5 || rank == 7 || rank == 9) return true;
        return false;
    }
}
