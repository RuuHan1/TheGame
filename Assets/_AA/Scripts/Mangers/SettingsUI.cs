using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UIPanel
{
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;


    private void Awake()
    {
        _sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 1f);
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        _sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicChanged);
    }
    private void OnDestroy()
    {
        _sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);
        _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
    }

    private void OnSfxChanged(float value)
    {
        GameEvents.SfxSliderChanged?.Invoke(value);
        PlayerPrefs.SetFloat("SfxVolume", value);
    }

    private void OnMusicChanged(float value)
    {
        GameEvents.MusicSliderChanged?.Invoke(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
