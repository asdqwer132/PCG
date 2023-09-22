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
                return "싱글";
            case GenealogyType.pair:
                return "페어";
            case GenealogyType.straightPair:
                return "스트레이트 페어";
            case GenealogyType.triple:
                return "트리플";
            case GenealogyType.straightTriple:
                return "스트레이트 트리플";
            case GenealogyType.fullHouse:
                return "풀하우스";
            case GenealogyType.straight:
                return "스트레이트";
            case GenealogyType.fourOfKind:
                return "포카드";
            case GenealogyType.straightFlush:
                return "스트레이트 플러쉬";
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
