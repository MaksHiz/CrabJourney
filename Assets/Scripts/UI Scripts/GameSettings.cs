using UnityEngine;

public static class GameSettings
{
    #region REGION: Enums
    public enum WindowSizeState
    {
        Small,
        Medium,
        Big
    }
    #endregion

    #region REGION: Public Properties
    // Master Volume Setting; when used, should be divided by 100f to get a float value between 0 and 1.
    private static readonly float MusicVolumeScale = 0.25f;
    private static readonly float SoundEffectsVolumeScale = 1.0f;


    public static int MasterVolume
    {
        get => GetPref("MasterVolume", 100);
        set 
        {
            SetPref("MasterVolume", value, 0, 100);
            AudioManager aud = GameObject.FindObjectOfType<AudioManager>();
            if (aud != null) 
            {
                aud.musicSource.volume = (MasterVolume / 100f) * (MusicVolume / 100f) * MusicVolumeScale;
                aud.loopSoundSource.volume = (MasterVolume / 100f) * (SoundEffectVolume / 100f) * SoundEffectsVolumeScale;
                aud.sfxSource.volume = (MasterVolume / 100f) * (SoundEffectVolume / 100f) * SoundEffectsVolumeScale;
            }
        }
    }

    // Music Volume Setting; when used, should be divided by 100f to get a float value between 0 and 1.
    public static int MusicVolume
    {
        get => GetPref("MusicVolume", 100);
        set
        {
            SetPref("MusicVolume", value, 0, 100);
            AudioManager aud = GameObject.FindObjectOfType<AudioManager>();
            if (aud != null)
            {
                aud.musicSource.volume = (MasterVolume / 100f) * (MusicVolume / 100f) * MusicVolumeScale;
            }
        }
    }

    // Sound Effect Volume Setting; when used, should be divided by 100f to get a float value between 0 and 1.
    public static int SoundEffectVolume
    {
        get => GetPref("SoundEffectVolume", 100);
        set
        {
            SetPref("SoundEffectVolume", value, 0, 100);
            AudioManager aud = GameObject.FindObjectOfType<AudioManager>();
            if (aud != null)
            {
                aud.loopSoundSource.volume = (MasterVolume / 100f) * (SoundEffectVolume / 100f) * SoundEffectsVolumeScale;
                aud.sfxSource.volume = (MasterVolume / 100f) * (SoundEffectVolume / 100f) * SoundEffectsVolumeScale;
            }
        }
    }

    // Fullscreen Setting.
    public static bool IsFullscreen
    {
        get => PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
            SetResolutionForFullscreen(value);
        }
    }

    // Window Size Setting.
    public static WindowSizeState WindowSize
    {
        get => (WindowSizeState)PlayerPrefs.GetInt("WindowSize", (int)WindowSizeState.Medium);
        set
        {
            PlayerPrefs.SetInt("WindowSize", (int)value);
            if (!IsFullscreen) AdjustWindowSize(value);
        }
    }
    #endregion

    #region Constructor
    static GameSettings()
    {
        SetResolutionForFullscreen(IsFullscreen);
    }
    #endregion

    #region Public Methods
    public static void ResetSoundToDefault()
    {
        MasterVolume = MusicVolume = SoundEffectVolume = 100;
    }
    #endregion

    #region Private Methods
    private static int GetPref(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);

    private static void SetPref(string key, int value, int min, int max)
    {
        PlayerPrefs.SetInt(key, Mathf.Clamp(value, min, max));
        PlayerPrefs.Save();
    }

    private static void SetResolutionForFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            AdjustWindowSize(WindowSize);
        }
        PlayerPrefs.Save();
    }

    private static void AdjustWindowSize(WindowSizeState state)
    {
        int width = state switch
        {
            WindowSizeState.Small => Mathf.RoundToInt(Screen.currentResolution.width * 0.5f),
            WindowSizeState.Medium => Mathf.RoundToInt(Screen.currentResolution.width * 0.75f),
            WindowSizeState.Big or _ => Screen.currentResolution.width,
        };

        int height = Mathf.RoundToInt(width / (16f / 9f));
        Screen.SetResolution(width, height, IsFullscreen);
    }
    #endregion
}
