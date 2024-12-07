using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    #region Public Properties

    public static float MasterVolume
    {
        get => _master_volume;
        set
        {
            _master_volume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat("MasterVolume", _master_volume);
            PlayerPrefs.Save();
        }
    }

    public static float MusicVolume
    {
        get => _music_volume;
        set
        {
            _music_volume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat("MusicVolume", _music_volume);
            PlayerPrefs.Save();
        }
    }

    public static float AmbientVolume
    {
        get => _ambient_volume;
        set
        {
            _ambient_volume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat("AmbientVolume", _ambient_volume);
            PlayerPrefs.Save();
        }
    }

    public static float SoundEffectVolume
    {
        get => _sound_effect_volume;
        set
        {
            _sound_effect_volume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat("SoundEffectVolume", _sound_effect_volume);
            PlayerPrefs.Save();
        }
    }

    public static Vector2Int Resolution
    {
        get
        {
            int width = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
            int height = Mathf.RoundToInt(width / (16f / 9f));
            return new Vector2Int(width, height);
        }
        set
        {
            int width = value.x;
            int height = Mathf.RoundToInt(width / (16f / 9f));
            PlayerPrefs.SetInt("ResolutionWidth", width);
            PlayerPrefs.SetInt("ResolutionHeight", height);
            Screen.SetResolution(width, height, IsFullscreen);
            PlayerPrefs.Save();
        }
    }

    public static bool IsFullscreen
    {
        get => PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
            Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.SetResolution(Resolution.x, Resolution.y, value);
            PlayerPrefs.Save();
        }
    }
    
    #endregion

    #region Private Fields

    private static float _master_volume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
    private static float _music_volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
    private static float _sound_effect_volume = PlayerPrefs.GetFloat("SoundEffectVolume", 1.0f);
    private static float _ambient_volume = PlayerPrefs.GetFloat("AmbientVolume", 1.0f);

    #endregion

    #region Public Methods

    //Constructor
    static GameSettings() 
    {

    }

    // Resets sound to default
    public static void ResetSoundToDefault()
    {
        MasterVolume = 1.0f;
        MusicVolume = 1.0f;
        SoundEffectVolume = 1.0f;
    }

    #endregion

    #region Private Methods

    #endregion
}
