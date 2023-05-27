using UnityEngine;
using TMPro;
using UnityEngine.Localization.Custom;
using System.Collections.Generic;

namespace Assets.Game.Scripts.UI
{
    public class InterfaceSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _languageDropdown;
        [SerializeField] private TMP_Dropdown _speedDropdown;
        [SerializeField] private TMP_Dropdown _temperatureDropdown;


        private void Awake()
        {
            _languageDropdown.value = (int)Localization.GetCurrentLanguage();

            _speedDropdown.onValueChanged.AddListener(OnSpeedValueChanged);
            _speedDropdown.value = PlayerPrefs.GetString("speedValue", "KMH") == "KMH" ? 0 : 1;
            _speedDropdown.onValueChanged.AddListener((value) => UIManager.Instance.ButtonSound());

            _temperatureDropdown.onValueChanged.AddListener(OnTemperatureValueChanged);
            _temperatureDropdown.value = PlayerPrefs.GetString("temperatureValue", "C") == "C" ? 0 : 1;
            _temperatureDropdown.onValueChanged.AddListener((value) => UIManager.Instance.ButtonSound());
        }

        public void OnSpeedValueChanged(int value) => PlayerPrefs.SetString("speedValue", value == 0 ? "KMH" : "MPH");

        public void OnTemperatureValueChanged(int value) => PlayerPrefs.SetString("temperatureValue", value == 0 ? "C" : "F");

    }
}
