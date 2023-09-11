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
    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(musicVolumeName, 0.75f);
        SetLevel(slider.value);

    }
    private void Update()
    {
        SetLevel(slider.value);
    }
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(musicVolumeName, Mathf.Log10(sliderValue) * 20 * bonus);
        PlayerPrefs.SetFloat(musicVolumeName, sliderValue);
    }
}