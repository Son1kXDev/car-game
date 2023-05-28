using UnityEngine;
using UnityEngine.UI;
using Assets.Game.Scripts.Data;

public class VolumeUIControll : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _ambientVolumeSlider;
    [SerializeField] private Slider _uiVolumeSlider;


    private void OnEnable()
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
        DataPersistenceManager.Instance.SaveSettings();
    }

    public void UpdateMusic()
    {
        AudioManager.Instance.MusicVolume = _musicVolumeSlider.value;
        DataPersistenceManager.Instance.SaveSettings();
    }

    public void UpdateSFX()
    {
        AudioManager.Instance.SFXVolume = _sfxVolumeSlider.value;
        DataPersistenceManager.Instance.SaveSettings();
    }

    public void UpdateAmbient()
    {
        AudioManager.Instance.AmbientVolume = _ambientVolumeSlider.value;
        DataPersistenceManager.Instance.SaveSettings();
    }

    public void UpdateUI()
    {
        AudioManager.Instance.UIVolume = _uiVolumeSlider.value;
        DataPersistenceManager.Instance.SaveSettings();
    }
}
