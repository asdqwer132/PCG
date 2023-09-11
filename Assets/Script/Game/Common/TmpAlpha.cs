using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TmpAlpha : MonoBehaviour
{
    [SerializeField] float lerpTime = 0.5f;
    [SerializeField] TextMeshProUGUI[] text;
    /*
     * 0. not your turn
     * 1. check your card
     * 2. you are not master
     */
    public void FadeOut(int index)
    {
        text[index].gameObject.SetActive(true);
        StartCoroutine(AlphaLerp(index, 1, 0));
    }
    IEnumerator AlphaLerp(int index, float start, float end)
    {
        float currentTIme = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
        {
            currentTIme += Time.deltaTime;
            percent = currentTIme / lerpTime;
            Color color = text[index].color;
            color.a = Mathf.Lerp(start, end, percent);
            text[index].color = color;
            yield return null;
        }
        text[index].gameObject.SetActive(false);
    }
}