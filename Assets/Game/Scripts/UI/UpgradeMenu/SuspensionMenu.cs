using System.Collections;
using System.Collections.Generic;
using Assets.Game.Scripts.Game;
using Assets.Game.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Game.Scripts.UI
{
    public class SuspensionMenu : MonoBehaviour, IDataPersistence
    {
        [SerializeField] Slider _stiffnessSlider;
        [SerializeField] Slider _heightSlider;

        [SerializeField] TextMeshProUGUI _costText;
        private int _currentCost;

        private float _stiffness, _height;

        private CarInfo _carInfo;
        private CarConfig _carConfig;
        private MenuCar _car;


        public void LoadData(GameData data)
        {
            _carConfig = FindFirstObjectByType<CarConfig>();
            _car = FindFirstObjectByType<MenuCar>();
            _carInfo = FindObjectOfType<CarInfo>(true);
            ResetData();
        }

        public void SaveData(GameData data)
        { }

        void OnEnable() => ResetData();

        public void ApplyData()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _carConfig.CurrentCarUpgrades.SuspensionFrequencyMultiplier = _stiffnessSlider.value;
                _carConfig.CurrentCarUpgrades.SuspensionHeightMultiplier = _heightSlider.value;
                if (_carConfig.CostsDictionary.ContainsKey("Suspension"))
                    _carConfig.CostsDictionary.Remove("Suspension");
                _carConfig.CostsDictionary.Add("Suspension", _currentCost);
                _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
                _carInfo.UpdateDisplayData();
                ResetData();
            }
        }

        public void UpdateData()
        {
            _car.UpdateSuspensionData(_stiffnessSlider.value, _heightSlider.value);
            _currentCost = (int)Mathf.Round((1000 * Mathf.Abs(_stiffness - _stiffnessSlider.value)) * 2.5f + (1000 * Mathf.Abs(_height - _heightSlider.value)) / 1.6f) * 2;
            _costText.text = _currentCost > 0 ? $"{_currentCost}  <sprite index=1>" : string.Empty;
            ApplyButton(_currentCost > 0);
        }

        void ResetData()
        {
            _stiffness = _stiffnessSlider.value = _carConfig.CurrentCarUpgrades.SuspensionFrequencyMultiplier;
            _height = _heightSlider.value = _carConfig.CurrentCarUpgrades.SuspensionHeightMultiplier;
            _costText.text = string.Empty;
            ApplyButton(false);
        }

        public void ApplyButton(bool value)
        {
            Image applyButton = transform.Find("ApplyButton").GetComponent<Image>();
            TextMeshProUGUI applyButtonText = applyButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>();

            applyButton.enabled = value;
            applyButtonText.enabled = value;
        }

    }
}
