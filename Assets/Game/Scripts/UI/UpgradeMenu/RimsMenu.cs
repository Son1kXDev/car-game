using Assets.Game.Scripts.Data;
using Assets.Game.Scripts.Game;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class RimsMenu : MonoBehaviour, IDataPersistence
    {
        [SerializeField, StatusIcon(offset: 20)] private Transform _propertyField;
        [SerializeField, StatusIcon(offset: 20)] private ScrollRect _scroll;
        [SerializeField] private List<GameObject> _rims;
        [SerializeField, StatusIcon(offset: 20)] private ActionButton _actionButton;
        private CarConfig _carConfig;
        private CarInfo _carInfo;
        private MenuCar _car;
        private List<int> _openedRims;

        private int _currentCost;
        private int _selectedID;

        public void LoadData(GameData data)
        {
            _carConfig = FindFirstObjectByType<CarConfig>();
            _car = FindFirstObjectByType<MenuCar>();
            _carInfo = FindObjectOfType<CarInfo>(true);
            _openedRims = data.OpenedRims;
        }

        public void SaveData(GameData data)
        {
            data.OpenedRims = _openedRims;
        }


        private void OnEnable()
        {
            SelectRim(_carConfig.CurrentRim);
        }

        public void SelectRim(int id)
        {
            _selectedID = id;
            StartCoroutine(_scroll.FocusAtPointCoroutine(_rims[id].transform.localPosition, 1.5f));

            _car.SelectRim(_selectedID);

            _propertyField.Find("Name").Find("Logo").GetComponent<Image>().sprite = _carConfig.CurrentCar.RimsIconsSprites[id];
            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _carConfig.CurrentCar.RimsNames[id];
            _currentCost = _carConfig.CurrentCar.RimsCost[id];
            _propertyField.Find("Cost").GetComponent<TextMeshProUGUI>().text = _currentCost.ToString(CustomStringFormat.CoinFormat(_currentCost)) + " <sprite index=1>";

            bool hasRim = false;
            foreach (int rims in _openedRims)
            {
                if (rims == id)
                {
                    hasRim = true;
                    break;
                }
            }

            if (_carConfig.CurrentRim == _selectedID)
                _actionButton.ButtonSetActive(ActionButtonType.Selected);
            else
            {
                if (hasRim)
                {
                    _actionButton.ButtonSetActive(ActionButtonType.Apply);
                    _actionButton.UpdateAction(() => ApplySelectedRim());
                }
                else
                {
                    _actionButton.ButtonSetActive(ActionButtonType.Buy);
                    _actionButton.UpdateAction(() => BuySelectedRim());
                }
            }
        }

        public void BuySelectedRim()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _openedRims.Add(_selectedID);
                _actionButton.ButtonSetActive(ActionButtonType.Apply);
                _actionButton.UpdateAction(() => ApplySelectedRim());
                DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ApplySelectedRim()
        {
            _car.SetRim(_selectedID);
            _actionButton.ButtonSetActive(ActionButtonType.Selected);
            if (_carConfig.CostsDictionary.ContainsKey("Rim"))
                _carConfig.CostsDictionary.Remove("Rim");
            _carConfig.CostsDictionary.Add("Rim", _currentCost);
            _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
            _carInfo.UpdateDisplayData();
        }
    }
}