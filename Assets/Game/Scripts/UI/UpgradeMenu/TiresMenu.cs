using Assets.Game.Scripts.Data;
using Assets.Game.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class TiresMenu : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private Transform _propertyField;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private List<GameObject> _tires;
        [SerializeField] private ActionButton _actionButton;

        private CarConfig _carConfig;
        private CarInfo _carInfo;
        private MenuCar _car;

        private List<int> _openedTires;

        private int _currentCost;
        private int _selectedID;

        public void LoadData(GameData data)
        {
            _carConfig = FindFirstObjectByType<CarConfig>();
            _car = FindFirstObjectByType<MenuCar>();
            _carInfo = FindObjectOfType<CarInfo>(true);
            _openedTires = data.OpenedTires;
        }

        public void SaveData(GameData data)
        {
            data.OpenedTires = _openedTires;
        }

        private void OnEnable()
        {
            SelectTire(_carConfig.CurrentTire);
        }

        public void SelectTire(int id)
        {
            _selectedID = id;
            StartCoroutine(_scroll.FocusAtPointCoroutine(_tires[id].transform.localPosition, 1.5f));

            _car.SelectTire(_selectedID);

            _propertyField.Find("Name").Find("Logo").GetComponent<Image>().sprite = _carConfig.CurrentCar.TiresIconsSprites[id];
            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _carConfig.CurrentCar.TiresNames[id];
            _currentCost = _carConfig.CurrentCar.TiresCost[id];
            _propertyField.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"{_currentCost} <sprite index=1>";

            bool hasTire = false;
            foreach (int rims in _openedTires)
            {
                if (rims == id)
                {
                    hasTire = true;
                    break;
                }
            }

            if (_carConfig.CurrentTire == _selectedID)
                _actionButton.ButtonSetActive(ActionButtonType.Selected);
            else
            {
                if (hasTire)
                {
                    _actionButton.ButtonSetActive(ActionButtonType.Apply);
                    _actionButton.UpdateAction(() => ApplySelectedTire());
                }
                else
                {
                    _actionButton.ButtonSetActive(ActionButtonType.Buy);
                    _actionButton.UpdateAction(() => BuySelectedTire());
                }
            }
        }

        public void BuySelectedTire()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _openedTires.Add(_selectedID);
                _actionButton.ButtonSetActive(ActionButtonType.Apply);
                _actionButton.UpdateAction(() => ApplySelectedTire());
                DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ApplySelectedTire()
        {
            _car.SetTire(_selectedID);
            _actionButton.ButtonSetActive(ActionButtonType.Selected);
            if (_carConfig.CostsDictionary.ContainsKey("Tire"))
                _carConfig.CostsDictionary.Remove("Tire");
            _carConfig.CostsDictionary.Add("Tire", _currentCost);
            _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
            _carInfo.UpdateDisplayData();
        }
    }
}