using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Game.Scripts;
using Assets.Game.Scripts.Game;

public class EngineMenu : MonoBehaviour
{
    [SerializeField] private int _maxUpgrades = 10;

    private CarConfig _carConfig;
    private TextMeshProUGUI _upgradeText;
    private TextMeshProUGUI _costText;
    private int _cost = 1230;
    private int _currentUpgrade;

    private void Awake()
    {
        _carConfig = FindFirstObjectByType<CarConfig>();
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
                _carConfig.CurrentCarUpgrades.EngineMultiplier = _currentUpgrade switch
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
            }
        }
    }
}