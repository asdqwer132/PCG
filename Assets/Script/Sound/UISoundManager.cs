using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public void ClickEffect()
    {
        int range = Random.Range(0, 3);
        SoundEffecter.Instance.PlayUIEffect(SoundType.UI,"click" + range);
    }
    public void StartEffect()
    {
        SoundEffecter.Instance.PlayEffect(SoundType.UI,"bell");
    }
    public void CardSelect()
    {
        int range = Random.Range(0, 0);
        SoundEffecter.Instance.PlayUIEffect(SoundType.card,"select" + range);
    }
}
