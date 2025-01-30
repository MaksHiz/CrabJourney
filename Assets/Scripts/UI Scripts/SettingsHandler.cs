using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider SoundEffectVolume;

    public TMP_Text MasterVolumeText;
    public TMP_Text MusicVolumeText;
    public TMP_Text SoundEffectVolumeText;

    public Toggle FullscreenToggle;
    public TMP_Dropdown WindowSizeDropdown; // Dropdown for window size

    void OnEnable()
    {
        MasterVolume.value = GameSettings.MasterVolume;
        MusicVolume.value = GameSettings.MusicVolume;
        SoundEffectVolume.value = GameSettings.SoundEffectVolume;

        MasterVolumeText.text = GameSettings.MasterVolume.ToString("0");
        MusicVolumeText.text = GameSettings.MusicVolume.ToString("0");
        SoundEffectVolumeText.text = GameSettings.SoundEffectVolume.ToString("0");

        FullscreenToggle.isOn = GameSettings.IsFullscreen;

        // Initialize window size dropdown
        WindowSizeDropdown.value = (int)GameSettings.WindowSize;
        WindowSizeDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        // Update master volume
        if (MasterVolume.value != GameSettings.MasterVolume)
        {
            GameSettings.MasterVolume = (int)MasterVolume.value;
            MasterVolumeText.text = GameSettings.MasterVolume.ToString("0");
        }

        // Update music volume
        if (MusicVolume.value != GameSettings.MusicVolume)
        {
            GameSettings.MusicVolume = (int)MusicVolume.value;
            MusicVolumeText.text = GameSettings.MusicVolume.ToString("0");
        }

        // Update sound effect volume
        if (SoundEffectVolume.value != GameSettings.SoundEffectVolume)
        {
            GameSettings.SoundEffectVolume = (int)SoundEffectVolume.value;
            SoundEffectVolumeText.text = GameSettings.SoundEffectVolume.ToString("0");
        }

        // Update fullscreen toggle
        if (FullscreenToggle.isOn != GameSettings.IsFullscreen)
        {
            GameSettings.IsFullscreen = FullscreenToggle.isOn;
        }

        // Update window size from dropdown
        if (WindowSizeDropdown.value != (int)GameSettings.WindowSize)
        {
            GameSettings.WindowSize = (GameSettings.WindowSizeState)WindowSizeDropdown.value;
        }
    }
}
