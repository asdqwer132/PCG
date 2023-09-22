using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SoundSetter : Popup
{
    [SerializeField] TextMeshProUGUI bgmText;
    [SerializeField] SoundSlider bgm;
    [SerializeField] SoundSlider effect;
    [SerializeField] int maxIndex = 0;
    int index = 0;
    private void Start()
    {
        bgm.SetLevel(true);
        effect.SetLevel(true);
        PlayMusicOperator.Instance.PlayBGM("bgm" + index);
        maxIndex = PlayMusicOperator.Instance.MaxListCount();
        Close();
    }
    public void SetText(string bgm)
    {
        bgmText.text = bgm;
    }
    public void ChangeBgm(bool isPlus)
    {
        index += isPlus ? 1 : -1;
        if (isPlus && index >= maxIndex) index = 0;
        if (!isPlus && index < 0) index = maxIndex - 1; 
        PlayMusicOperator.Instance.PlayBGM("bgm" + index);
    }
}
