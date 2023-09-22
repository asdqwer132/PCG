using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SZWinnerDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nicknameText;
    [SerializeField] TextMeshProUGUI scoreText;

    public void Setup(int score, string nickname)
    {
        nicknameText.text = nickname;
        scoreText.text = score.ToString();
    }
}
