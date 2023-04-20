using UnityEngine;
using UnityEngine.UI;

public class VolumeUIControll : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    void Awake()
    {
        _masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
        _musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
        _sfxVolumeSlider.value = AudioManager.Instance.SFXVolume;
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
}
