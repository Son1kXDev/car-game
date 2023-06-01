using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization.Custom;
using TMPro;
using Assets.Game.Scripts.Data;

namespace Assets.Game.Scripts.UI
{
    public class GraphicsSettings : MonoBehaviour, ISettingsDataPersistence
    {
        [SerializeField, StatusIcon(offset: 20)] private TMP_Dropdown _qualityDropdown;
        [SerializeField, StatusIcon(offset: 20)] private TextToggle _vsyncToggle;
        [SerializeField, StatusIcon(offset: 20)] private TextToggle _particlesToggle;

        private int _quality;
        private bool _vsync;
        private bool _particles;

        private void ApplySettings()
        {
            UpdateQualityDropdown();

            _vsyncToggle.onValueChanged.AddListener(SetVsync);
            _vsyncToggle.isOn = _vsync;

            _particlesToggle.onValueChanged.AddListener(SetParticles);
            _particlesToggle.isOn = _particles;
        }

        public void LoadData(SettingsData data)
        {
            _quality = data.Quality;
            _vsync = data.VSync;
            _particles = data.Particles;
            ApplySettings();
        }

        public void SaveData(SettingsData data)
        {
            data.Quality = _quality;
            data.VSync = _vsync;
            data.Particles = _particles;
        }


        private void Start() => GlobalEventManager.Instance.OnLanguageChanged += UpdateQualityDropdown;

        private void OnEnable() => UpdateQualityDropdown();

        private void OnDestroy() => GlobalEventManager.Instance.OnLanguageChanged -= UpdateQualityDropdown;

        public void UpdateQualityDropdown()
        {
            List<string> options = new List<string>();
            options.Add(Localization.GetCurrentLanguage() == Lang.English ? "Low" : "Низкая");
            options.Add(Localization.GetCurrentLanguage() == Lang.English ? "Medium" : "Средняя");
            options.Add(Localization.GetCurrentLanguage() == Lang.English ? "High" : "Высокая");

            _qualityDropdown.onValueChanged.RemoveAllListeners();
            _qualityDropdown.ClearOptions();
            _qualityDropdown.AddOptions(options);

            _qualityDropdown.value = _quality;
            _qualityDropdown.RefreshShownValue();
            _qualityDropdown.onValueChanged.AddListener(SetQuality);
            _qualityDropdown.onValueChanged.AddListener((value) => UIManager.Instance.ButtonSound());
        }


        public void SetQuality(int quality)
        {
            QualitySettings.SetQualityLevel(quality);
            _quality = quality;
            DataPersistenceManager.Instance.SaveSettings();
        }

        public void SetVsync(bool vsync)
        {
            QualitySettings.vSyncCount = vsync ? 1 : 0;
            _vsync = vsync;
            DataPersistenceManager.Instance.SaveSettings();
        }

        public void SetParticles(bool enabled)
        {
            _particles = enabled;
            DataPersistenceManager.Instance.SaveSettings();
            GlobalEventManager.Instance.ParticleToggleChanged();
        }
    }
}
