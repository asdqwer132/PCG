using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TichuLanternManager : MonoBehaviour
{

    [SerializeField] List<Lantern> turnLantern = new List<Lantern>();
    [SerializeField] List<Lantern> passLantern = new List<Lantern>();

    public void SetTurnLantern(int index, bool isOn) { turnLantern[index].SetColor(ColorSelector.OnOffColor(isOn)); }
    public void SetPassLantern(int index, bool isOn)
    {
        if (index != 0)
        {
            passLantern[index].gameObject.SetActive(isOn);
        }
        else passLantern[index].SetColor(ColorSelector.OnOffColor(isOn));
    }
    public void ResetPassLanter() { for (int i = 0; i < passLantern.Count; i++) { SetPassLantern(i, false); } }
    public void ResetTurnLanter() { for (int i = 0; i < turnLantern.Count; i++) { SetTurnLantern(i, false); } }
}
