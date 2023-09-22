using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextRepeater : MonoBehaviour
{
    [SerializeField] string[] textScript;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float term = 0.5f;

    private void Start() { StartRepeat(); }
    public void StopRepeat() { StopCoroutine("repeater"); }
    public void StartRepeat() { StartCoroutine("repeater"); }
    public void SetText(string script) { text.text = script; }
    public void SetColor(Color color) { text.color = color; }
    IEnumerator repeater()
    {
        int i = 0;
        while (gameObject.activeSelf)
        {
            SetText(textScript[i++]);
            yield return new WaitForSeconds(term);
            if (i == textScript.Length) i = 0;
        }
    }
}
