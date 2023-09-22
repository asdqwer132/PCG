using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] string musicVolumeName;//
    [SerializeField] float bonus = 1f;
    private void Update()
    {
        SetLevel(false);
    }
    public void SetLevel(bool isStart)
    {
        if(isStart) slider.value = PlayerPrefs.GetFloat(musicVolumeName, 0.75f);
        mixer.SetFloat(musicVolumeName, Mathf.Log10(slider.value) * 20 * bonus);
        PlayerPrefs.SetFloat(musicVolumeName, slider.value);
    }
}