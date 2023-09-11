using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BgmType
{
    public string name;
    public AudioClip audio;
}

public class PlayMusicOperator : MonoBehaviour
{
    [SerializeField] string playBGM;
    public BgmType[] BGMList;

    public AudioSource BGM;
    private string NowBGMname = "";
    void Start()
    {
        if (BGMList.Length > 0)
        {
            PlayBGM(BGMList[0].name);
            if (playBGM != null) PlayBGM(playBGM);
        }
    }

    public void PlayBGM(string name)
    {
        if (NowBGMname.Equals(name)) return;

        for (int i = 0; i < BGMList.Length; ++i)
            if (BGMList[i].name.Equals(name))
            {
                BGM.Stop();
                BGM.clip = BGMList[i].audio;
                BGM.Play();
                NowBGMname = name;
            }
    }
}