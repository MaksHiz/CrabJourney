using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region VOLUME HANDLING

    // Functions for handling master_volume
    private float master_volume = 1.0f;
    public float GetMasterVolume() { return master_volume; }
    public bool SetMasterVolume(float volume)
    {
        if (volume <= 1f && volume >= 0f)
        {
            master_volume = volume;
            return true;
        }
        else return false;
    }

    // Functions for handling music_volume
    private float music_volume = 1.0f;
    public float GetMusicVolume() { return music_volume; }
    public bool SetMusicVolume(float volume)
    {
        if (volume <= 1f && volume >= 0f)
        {
            music_volume = volume;
            return true;
        }
        else return false;
    }

    // Functions for handling sound_effect_volume
    private float sound_effect_volume = 1.0f;
    public float GetSoundEffectVolume() { return sound_effect_volume; }
    public bool SetSoundEffectVolume(float volume)
    {
        if (volume <= 1f && volume >= 0f)
        {
            sound_effect_volume = volume;
            return true;
        }
        else return false;
    }

    // Functions for handling ui_volume
    private float ui_volume = 1.0f;
    public float GetUIVolume() { return ui_volume; }
    public bool SetUIVolume(float volume)
    {
        if (volume <= 1f && volume >= 0f)
        {
            ui_volume = volume;
            return true;
        }
        else return false;
    }

    // Functions for handling muted
    private bool muted = false;
    public bool IsMuted() { return muted; }
    public void Mute(bool mute)
    {
        muted = mute;
    }

    #endregion

    #region AUDIOSOURCE HANDLING

    //@TODO implement music_source functions for the queue.
    private AudioSource music_source;
    private AudioSource ui_source;
    private AudioSource effects_source;

    #endregion

    #region ONESHOT FUNCTIONS

    public void PlayOneShotSoundEffect(AudioClip clip, float volume = 1.0f) 
    {
        if (clip != null)
        {
            effects_source.PlayOneShot(clip, master_volume * sound_effect_volume * volume);
        }
        else 
        {
            Debug.LogWarning("Attempted to play a sound effect but the clip was null.");
        }
    }
    public void PlayOneShotUIEffect(AudioClip clip, float volume = 1.0f) 
    {
        if (clip != null)
        {
            ui_source.PlayOneShot(clip, master_volume * ui_volume * volume);
        }
        else
        {
            Debug.LogWarning("Attempted to play a UI sound but the clip was null.");
        }
    }

    #endregion
}
