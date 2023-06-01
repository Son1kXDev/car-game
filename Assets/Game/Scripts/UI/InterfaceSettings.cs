using UnityEngine;
using TMPro;
using UnityEngine.Localization.Custom;
using System.Collections.Generic;
using Assets.Game.Scripts.Data;

namespace Assets.Game.Scripts.UI
{
    public class InterfaceSettings : MonoBehaviour, ISettingsDataPersistence
    {
        [SerializeField, StatusIcon(offset: 20)] private TMP_Dropdown _languageDropdown;
        [SerializeField, StatusIcon(offset: 20)] private TMP_Dropdown _speedDropdown;
        [SerializeField, StatusIcon(offset: 20)] private TMP_Dropdown _temperatureDropdown;

        private void Awake()
        {
            _languageDropdown.value = (int)Localization.GetCurrentLanguage();
            DataPersistenceManager.Instance.LoadSettings();
        }

        public void LoadData(SettingsData data)
        {
            _speedDropdown.onValueChanged.RemoveAllListeners();
            _speedDropdown.value = (int)data.SpeedValue;
            _speedDropdown.RefreshShownValue();
            _speedDropdown.onValueChanged.AddListener(OnValueChanged);
            _speedDropdown.onValueChanged.AddListener((value) => UIManager.Instance.ButtonSound());

            _temperatureDropdown.onValueChanged.RemoveAllListeners();
            _temperatureDropdown.value = (int)data.Temperature;
            _temperatureDropdown.RefreshShownValue();
            _temperatureDropdown.onValueChanged.AddListener(OnValueChanged);
            _temperatureDropdown.onValueChanged.AddListener((value) => UIManager.Instance.ButtonSound());
        }

        public void SaveData(SettingsData data)
        {
            data.SpeedValue = (Speed)_speedDropdown.value;
            data.Temperature = (Temp)_temperatureDropdown.value;
        }

        public void OnValueChanged(int value) => DataPersistenceManager.Instance.SaveSettings();

    }
}
