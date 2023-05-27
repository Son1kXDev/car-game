using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization.Custom;
using TMPro;

namespace Assets.Game.Scripts.UI
{
    public class GraphicsSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private TextToggle _vsyncToggle;
        [SerializeField] private TextToggle _particlesToggle;


        private void Awake()
        {
            UpdateQualityDropdown();

            _vsyncToggle.onValueChanged.AddListener(SetVsync);
            _vsyncToggle.isOn = bool.Parse(PlayerPrefs.GetString("VSync", "true"));

            _particlesToggle.onValueChanged.AddListener(SetParticles);
            _particlesToggle.isOn = bool.Parse(PlayerPrefs.GetString("particlesToggle", "true"));
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

            _qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
            _qualityDropdown.RefreshShownValue();
            _qualityDropdown.onValueChanged.AddListener(SetQuality);
            _qualityDropdown.onValueChanged.AddListener((value) => UIManager.Instance.ButtonSound());
        }


        public void SetQuality(int quality)
        {
            QualitySettings.SetQualityLevel(quality);
            PlayerPrefs.SetInt("Quality", quality);
        }

        public void SetVsync(bool vsync)
        {
            QualitySettings.vSyncCount = vsync ? 1 : 0;
            PlayerPrefs.SetString("VSync", vsync.ToString());
        }

        public void SetParticles(bool enabled)
        {
            PlayerPrefs.SetString("particlesToggle", enabled.ToString());
            GlobalEventManager.Instance.ParticleToggleChanged();
        }

    }
}
