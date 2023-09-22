using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lantern : MonoBehaviour
{
    [SerializeField] Image image;
    private void Awake() { image = GetComponent<Image>(); }
    public void SetColor(Color color) { image.color = color; }
    public void SetImage(Sprite sprite) { image.sprite = sprite; }
}
