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
}
