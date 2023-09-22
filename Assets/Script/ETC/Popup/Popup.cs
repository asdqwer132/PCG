using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    protected bool isOpen = false;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Toggle()
    {
        isOpen = !isOpen;
        gameObject.SetActive(isOpen);
    }
    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
