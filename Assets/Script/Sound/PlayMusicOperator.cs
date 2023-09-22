using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BgmType
{
    public string indexName;
    public AudioClip audio;
}

public class PlayMusicOperator : MonoBehaviour
{
    [SerializeField] SoundSetter soundSetter;
    public List<BgmType> BGMList;

    static PlayMusicOperator instance;
    public static PlayMusicOperator Instance { get => instance; set => instance = value; }

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
    public AudioSource BGM;
    private string NowBGMname = "";
    public void ResetBgm() { PlayBGM(BGMList[0].indexName); }
    public int MaxListCount() { return BGMList.Count; }
    public void PlayBGM(string name)
    {
        if (NowBGMname.Equals(name)) return;
        for (int i = 0; i < BGMList.Count; ++i)
        {
            if (BGMList[i].indexName.Equals(name))
            {
                BGM.Stop();
                BGM.clip = BGMList[i].audio;
                BGM.Play();
                NowBGMname = name;
                soundSetter.SetText(BGMList[i].audio.name);
            }
        }
    }
}