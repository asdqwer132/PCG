using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextChanger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI thisText;//
    private void Start() { thisText = GetComponent<TextMeshProUGUI>(); }
    public void SetText(string text) { thisText.text = text; }
    public void SetColor(Color color) { thisText.color = color; }
}
