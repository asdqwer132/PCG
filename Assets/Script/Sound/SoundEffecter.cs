using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct EffectList
{
    public List<BgmType> bgmTypes;
}
public class SoundEffecter : MonoBehaviour
{
    static SoundEffecter instance;
    [SerializeField] List<SoundType> soundTypeList;
    [SerializeField] List<EffectList> soundList;
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource BGMUI;
    Dictionary<SoundType, EffectList> soundEffectList = new Dictionary<SoundType, EffectList>();
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
        if(soundList.Count != soundTypeList.Count) { Debug.LogError("카운트 안맞잖아!" + soundList.Count  + "/" + soundTypeList.Count); }
        for(int i = 0; i < soundTypeList.Count; i++)
        {
            soundEffectList.Add(soundTypeList[i], soundList[i]);
        }
    }
    public void PlayEffectUIByRandom(SoundType soundType, string effectType, int max)
    {
        int range = Random.Range(0, max);
        PlayUIEffect(soundType, effectType + range);
    }
    public void PlayEffect(SoundType soundType ,string effectType)
    {
        BGM.clip = soundEffectList[soundType].bgmTypes.Find(x => x.indexName == effectType).audio;
        BGM.Play();
    }
    public void PlayUIEffect(SoundType soundType, string effectType)
    {
        BGMUI.clip = soundEffectList[soundType].bgmTypes.Find(x => x.indexName == effectType).audio;
        BGMUI.Play();
    }
    public void PlayeEffectByResources(string effectType)
    {
        BGM.clip = Resources.Load<AudioClip>("Audio/Tichu/" + effectType);
        BGM.Play();
    }
}