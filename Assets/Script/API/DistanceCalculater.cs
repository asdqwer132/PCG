using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculater
{
    public static int GetDistance(int turn)
    {

        int root = TichuPlayerManager.GetPlayerOwn().Index;
        if (turn - root >= 0) return turn - root;
        return 4 - (root - turn);
    }
    public static int GetDistanceWithRoot(int root, int turn)
    {
        if (turn - root >= 0) return turn - root;
        return 4 - (root - turn);
    }
}
