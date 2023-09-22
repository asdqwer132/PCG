using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenConverter : MonoBehaviour
{
    public static bool IsEven(int rank) { return rank % 2 == 0; }

    public static int GetIndexByIsEven(int rank) { int result = IsEven(rank) ? 0 : -1; return result; }
}
