using UnityEngine;
using UnityEngine.UI;

public class VolumeUIControll : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _ambientVolumeSlider;
    [SerializeField] private Slider _uiVolumeSlider;

    private void Start()
    {
        _masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
        _musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
        _sfxVolumeSlider.value = AudioManager.Instance.SFXVolume;
        _ambientVolumeSlider.value = AudioManager.Instance.AmbientVolume;
        _uiVolumeSlider.value = AudioManager.Instance.UIVolume;
    }

    public void UpdateMaster()
    {
        AudioManager.Instance.MasterVolume = _masterVolumeSlider.value;
        PlayerPrefs.SetFloat("MasterVolume", _masterVolumeSlider.value);
    }

    public void UpdateMusic()
    {
        AudioManager.Instance.MusicVolume = _musicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", _musicVolumeSlider.value);
    }

    public void UpdateSFX()
    {
        AudioManager.Instance.SFXVolume = _sfxVolumeSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolumeSlider.value);
    }

    public void UpdateAmbient()
    {
        AudioManager.Instance.AmbientVolume = _ambientVolumeSlider.value;
        PlayerPrefs.SetFloat("AmbientVolume", _ambientVolumeSlider.value);
    }

    public void UpdateUI()
    {
        AudioManager.Instance.UIVolume = _uiVolumeSlider.value;
        PlayerPrefs.SetFloat("UIVolume", _uiVolumeSlider.value);
    }
}
