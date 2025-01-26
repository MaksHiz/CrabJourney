using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip[] musicSounds, sfxSounds, loopingSounds;
    public AudioSource musicSource, sfxSource, loopSoundSource;
    private AudioClip whaleSound;
    private int musicIndex=0;
    private float currentTime = 0f;
    private float playWhaleTime = 5f;
    private void Awake()
    {
        Instance=this;
    }

    private void Start()
    {
        Debug.Log("Playing first song");
        Debug.Log(musicSounds[musicIndex]);
        PlayMusic(musicIndex);
        whaleSound = Array.Find(sfxSounds, x => x.name == "Whale_Sound");
        Debug.Log("WHALE SOUND: " + whaleSound);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (!musicSource.isPlaying)
        {
            musicIndex++;
            if (musicIndex >= musicSounds.Length)
                musicIndex = 0;
            PlayMusic(musicIndex);
        }
        if (musicIndex == 0 && whaleSound != null && currentTime >= playWhaleTime)
        {
            Debug.Log("PLaying Whale Sound");
            PlaySFX("Whale_Sound");
            playWhaleTime = UnityEngine.Random.Range(20, 25);
            currentTime = 0f;
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
            Debug.Log("Playing:" + name);
            sfxSource.clip = playedSFX;
            sfxSource.PlayOneShot(playedSFX);
        }

    }
    public void PlaySFX(string name,float delay)
    {
        AudioClip playedSFX = Array.Find(sfxSounds, x => x.name == name);

        if (playedSFX == null)
        {
            Debug.Log("No sfx found");
        }
        else
        {
            sfxSource.clip = playedSFX;
            sfxSource.PlayDelayed(delay);
        }
    }
    public void PlayLoopedSound(string name)
    {
        AudioClip playedLooped = Array.Find(loopingSounds, x => x.name == name);
        loopSoundSource.clip = playedLooped;
        loopSoundSource.enabled = true;
        loopSoundSource.Play();
    }
    public void StopSFX()
    {
        if (sfxSource.isPlaying) { sfxSource.Stop(); }
    }
    public void StopMusic()
    {
        if (musicSource.isPlaying) { musicSource.Stop(); }
    }
    public void StopLoopedSound()
    {
        loopSoundSource.enabled = false;
    }
}
