using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    //효과음
    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx 
    {
        Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win
    }


    void Awake()
    {
        instance = this;
        this.Init();
    }
    
    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;  //쉬고 있는 player를 만나면 그냥 넘어감
            }

            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                //
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            //효과음 2개 이상인 것은 랜덤 인덱스를 더하기
            sfxPlayers[0].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[0].Play();
            break;
        }
    }
}
