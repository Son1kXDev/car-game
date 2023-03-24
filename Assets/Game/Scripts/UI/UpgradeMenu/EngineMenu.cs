using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Game.Scripts;
using Assets.Game.Scripts.Game;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class EngineMenu : MonoBehaviour
    {
        [SerializeField] private int _maxUpgrades = 10;
        [SerializeField] private TextMeshProUGUI _upgradeText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private List<int> _costs;

        private CarConfig _carConfig;
        private CarInfo _carInfo;

        private int _cost;
        private int _currentUpgrade;

        private void OnEnable()
        {
            _carConfig = FindFirstObjectByType<CarConfig>();
            _carInfo = FindObjectOfType<CarInfo>(true);
            _currentUpgrade = _carConfig.CurrentCarUpgrades.EngineMultiplier switch
            {
                1f => 0,
                1.1f => 1,
                1.2f => 2,
                1.3f => 3,
                1.4f => 4,
                1.5f => 5,
                1.6f => 6,
                1.7f => 7,
                1.8f => 8,
                1.9f => 9,
                2f => 10,
                _ => 0
            };
            if (_currentUpgrade == 10)
            {
                Color transparent = new Color(0, 0, 0, 0);

                _costText.color = transparent;
                _upgradeButton.interactable = false;
                _upgradeButton.GetComponent<Image>().color = transparent;
                _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().color = transparent;
            }
            _cost = _costs[_currentUpgrade];
            _upgradeText.text = $"{_currentUpgrade}/{_maxUpgrades} upgrades";
            _costText.text = $"{_cost} <sprite index=1>";
        }

        public void Upgrade()
        {
            if (_currentUpgrade < _maxUpgrades)
            {
                if (CoinManager.Instance.DecreaseCoins(_cost))
                {
                    _currentUpgrade++;
                    _carConfig.CurrentCarUpgrades.EngineMultiplier = _carConfig.CurrentCarUpgrades.AccelirationMultiplier = _currentUpgrade switch
                    {
                        0 => 1f,
                        1 => 1.1f,
                        2 => 1.2f,
                        3 => 1.3f,
                        4 => 1.4f,
                        5 => 1.5f,
                        6 => 1.6f,
                        7 => 1.7f,
                        8 => 1.8f,
                        9 => 1.9f,
                        10 => 2f,
                        _ => 0
                    };
                    _cost = _costs[_currentUpgrade];
                    _upgradeText.text = $"{_currentUpgrade}/{_maxUpgrades} upgrades";
                    _costText.text = $"{_cost} <sprite index=1>";
                    if (_currentUpgrade == 10)
                    {
                        Color transparent = new Color(0, 0, 0, 0);

                        _costText.color = transparent;
                        _upgradeButton.interactable = false;
                        _upgradeButton.GetComponent<Image>().color = transparent;
                        _upgradeButton.GetComponentInChildren<TextMeshProUGUI>().color = transparent;
                    }
                    string key = $"Engine {_currentUpgrade}";
                    _carConfig.CostsDictionary.Add(key, _cost);
                    _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
                    _carInfo.UpdateDisplayData();
                    Data.DataPersistenceManager.Instance.SaveGame();
                }
            }
        }
    }
}