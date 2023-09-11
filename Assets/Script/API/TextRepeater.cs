using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextRepeater : MonoBehaviour
{
    [SerializeField] string[] textScript;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float term = 0.5f;

    private void Start()
    {
        StartCoroutine("repeater");
    }
    IEnumerator repeater()
    {
        int i = 0;
        while (gameObject.activeSelf)
        {
            text.text = textScript[i++];
            yield return new WaitForSeconds(term);
            if (i == textScript.Length) i = 0;
        }
    }
}
