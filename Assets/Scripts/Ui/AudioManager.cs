using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds,sfxSounds;
    public AudioSource musicSource,sfxSource;
    void Awake()
    {
        Instance = this;
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds,x => x.Name == name);
        if(s == null)
        {
            Debug.LogError("Mahni Tapilmadi");
        }
        else
        {
            musicSource.clip = s.audioClip;
            musicSource.volume = s.Volume;
            musicSource.Play();
        }
    }
    public void PlaySfx(string name)
    {
        Sound s = Array.Find(sfxSounds,x => x.Name == name);
        if(s == null)
        {
            Debug.LogError("Mahni Tapilmadi");
        }
        else
        {
            sfxSource.volume = s.Volume;
            sfxSource.PlayOneShot(s.audioClip);
        }
    }
}
