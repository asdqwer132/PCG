using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LanternManager : MonoBehaviour
{

    [SerializeField] List<Image> turnLantern = new List<Image>();
    [SerializeField] List<Image> passLantern = new List<Image>();

    public void SetTurnLantern(int index, bool isOn) { turnLantern[index].color = ColorSelector.OnOffColor(isOn); }
    public void SetPassLantern(int index, bool isOn)
    {
        if (index != 0)
        {
            passLantern[index].gameObject.SetActive(isOn);
        }
        else passLantern[index].color = ColorSelector.OnOffColor(isOn);
    }
    public void ResetPassLanter() { for (int i = 0; i < passLantern.Count; i++) { SetPassLantern(i, false); } }
    public void ResetTurnLanter() { for (int i = 0; i < turnLantern.Count; i++) { SetTurnLantern(i, false); } }
}
