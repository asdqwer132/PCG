using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TichuModeManager : MonoBehaviour
{
    [SerializeField] GameObject stMode;
    static bool isStMode = true;
    public static bool GetStMode() { return isStMode; } 
    public void SetMode()
    {
        isStMode = !isStMode;
        stMode.SetActive(isStMode);
    }
}
