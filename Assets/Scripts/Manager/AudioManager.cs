using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理音乐播放
/// </summary>
public class AudioManager
{
    private AudioSource[] audioSource;
    private bool playEffectMusic = true;
    private bool playBGMusic = true;

    public AudioManager()
    {
        audioSource = GameManager.instance.GetComponents<AudioSource>();
    }

    // 播放背景音乐
    public void PlayBGMusic(AudioClip audioClip)
    {
        if (!audioSource[0].isPlaying || audioClip != audioSource[0].clip)
        {
            audioSource[0].clip = audioClip;
            audioSource[0].Play();
        }
    }

    // 播放音效
    public void PlayEffectMusic(AudioClip audioClip)
    {
        if (playEffectMusic) audioSource[1].PlayOneShot(audioClip);
    }

    public void CloseBGMusic()
    {
        audioSource[0].Stop();
    }

    public void OpenBGMusic()
    {
        audioSource[0].Play();
    }

    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;
        if (playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }

    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
    }

    //按钮音效播放
    public void PlayButtonAudioClip()
    {
        PlayEffectMusic(GameManager.instance.assetManager.audioClipsFactory.GetSingleResources("Main/Button"));
    }
    //翻书音效播放
    public void PlayPagingAudioClip()
    {
        PlayEffectMusic(GameManager.instance.assetManager.audioClipsFactory.GetSingleResources("Main/Paging"));
    }
}
