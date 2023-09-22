using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NicknameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    public static string nickname;

    private void Start()
    {
        nickname = PlayerPrefs.GetString("nickname", "I AM IDIOT!"); 
        inputField.text = nickname;
    }
    public void SetNickname() { nickname = inputField.text; PlayerPrefs.SetString("nickname", inputField.text); }
}
