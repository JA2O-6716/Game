using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmplayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channles;
    AudioSource[] sfxplayers;
    public int channlesIndex;

    public enum Sfx { Dead = 0, Hit, LevelUp = 3 , Lose, Melee, Range = 7, Select, win }

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmobject = new GameObject("bgmplayer");
        bgmobject.transform.parent = transform;
        bgmplayer = bgmobject.AddComponent<AudioSource>();
        bgmplayer.playOnAwake = false;
        bgmplayer.loop = true;
        bgmplayer.volume = bgmVolume;
        bgmplayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        //효과음 플레이어 초기화
        GameObject sfxobject = new GameObject("sfxplayers");
        sfxobject.transform.parent = transform;
        sfxplayers = new AudioSource[channles];

        for(int i = 0; i < sfxplayers.Length; i++) 
        {
            sfxplayers[i] = sfxobject.AddComponent<AudioSource>();
            sfxplayers[i].playOnAwake = false;
            sfxplayers[i].bypassListenerEffects = true;
            sfxplayers[i].volume = sfxVolume;
        }
    }

    public void playSfx(Sfx sfx)
    {
        for(int i = 0; i < sfxplayers.Length;i++) 
        {
            int loopIndex = (i + channlesIndex) % sfxplayers.Length;

            if (sfxplayers[loopIndex].isPlaying)
            {
                continue;
            }

            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channlesIndex = loopIndex;
            sfxplayers[loopIndex].clip = sfxClip[(int)sfx];
            sfxplayers[loopIndex].Play();
            break;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if(isPlay)
        {
            bgmplayer.Play();
        }
        else
        {
            bgmplayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }
}
