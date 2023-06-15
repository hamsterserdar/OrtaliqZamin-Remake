using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    int sayi;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void NovbetiMahni()
    {
        sayi++;
        audioSource.clip = audioClips[sayi];
        audioSource.Play();
    }
}
