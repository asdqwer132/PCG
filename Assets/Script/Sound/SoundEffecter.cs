using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundEffecter : MonoBehaviour
{
    static SoundEffecter instance;
    [SerializeField] List<BgmType> soundList;
    [SerializeField] AudioSource BGM;

    public static SoundEffecter Instance { get => instance; set => instance = value; }
    private void OnDestroy()
    {
        instance = null;
    }
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    public void PlayEffect(string effectType)
    {
        BGM.clip = soundList.Find(x => x.name == effectType).audio;
        BGM.Play();
    }
    public void PlayeEffectByResources(string effectType)
    {
        BGM.clip = Resources.Load<AudioClip>("Audio/Tichu/" + effectType);
        BGM.Play();
    }
}