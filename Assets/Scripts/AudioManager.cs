using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private int musicIndex=0;


    private void Awake()
    {
        Instance=this;
    }

    private void Start()
    {
        Debug.Log("Playing first song");
        Debug.Log(musicSounds[musicIndex]);
        PlayMusic(musicIndex);
    }

    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            musicIndex++;
            if (musicIndex >= musicSounds.Length)
                musicIndex = 0;
            PlayMusic(musicIndex);
        }
    }

    public void PlayMusic(int index)
    {
        AudioClip playedMusic = musicSounds[index];

        if (playedMusic == null) {
            Debug.Log("No music found");
        }
        else
        {
            musicSource.clip = playedMusic;
            musicSource.PlayDelayed(2.5f);
        }

    }
    public void PlaySFX(string name)
    {
        AudioClip playedSFX = Array.Find(sfxSounds, x=>x.name==name);

        if (playedSFX == null)
        {
            Debug.Log("No sfx found");
        }
        else
        {
            sfxSource.clip = playedSFX;
            sfxSource.PlayOneShot(playedSFX);
        }

    }
    public void StopSFX()
    {
        if (sfxSource.isPlaying) { sfxSource.Stop(); }
    }
}
