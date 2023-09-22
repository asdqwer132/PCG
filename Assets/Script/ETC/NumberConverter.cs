using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberConverter
{
    public static string ConvertNum(float number)
    {
        if (number == 11) return "J";
        if (number == 12) return "Q";
        if (number == 13) return "K";
        if (number == 14) return "A";
        if (number == (int)Animal.dog) return "Dog";
        if (number == (int)Animal.bird) return "1";
        if (number == (int)Animal.dragon) return "Dragon";
        if (number == (int)Animal.phoenix) return "Phoenix";
        return number.ToString();
    }
    public static string GenealogyConvert(GenealogyType genealogyType)
    {
        switch (genealogyType)
        {
            case GenealogyType.none:
                return "";
            case GenealogyType.single:
                return "�̱�";
            case GenealogyType.pair:
                return "���";
            case GenealogyType.straightPair:
                return "��Ʈ����Ʈ ���";
            case GenealogyType.triple:
                return "Ʈ����";
            case GenealogyType.straightTriple:
                return "��Ʈ����Ʈ Ʈ����";
            case GenealogyType.fullHouse:
                return "Ǯ�Ͽ콺";
            case GenealogyType.straight:
                return "��Ʈ����Ʈ";
            case GenealogyType.fourOfKind:
                return "��ī��";
            case GenealogyType.straightFlush:
                return "��Ʈ����Ʈ �÷���";
        }
        return "";
    }
    public static int RankConverter(GameType gameType, int rank)
    {
        if (gameType == GameType.Tichu_4p) return rank - 2;
        if (gameType == GameType.NineDragon) return rank + 1;
        if (gameType == GameType.Suzume)
        {
            if (rank == 11) return 9;
            if (rank == 13) return 9;
            return rank - 1;
        }
        return 0;
    }
}
