using Assets.Game.Scripts.Data;
using Assets.Game.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class SpoilerMenu : MonoBehaviour, IDataPersistence
    {
        [SerializeField, StatusIcon] private Transform _propertyField;
        [SerializeField, StatusIcon] private ScrollRect _scroll;
        [SerializeField] private List<GameObject> _spoilers;
        [SerializeField, StatusIcon] private ActionButton _actionButton;

        private CarConfig _carConfig;
        private CarInfo _carInfo;
        private MenuCar _car;

        private List<int> _openedSpoilers;

        private int _currentCost;
        private int _selectedID;

        public void LoadData(GameData data)
        {
            _carConfig = FindFirstObjectByType<CarConfig>();
            _car = FindFirstObjectByType<MenuCar>();
            _carInfo = FindObjectOfType<CarInfo>(true);
            _openedSpoilers = data.OpenedSpoilers;
        }

        public void SaveData(GameData data)
        {
            data.OpenedSpoilers = _openedSpoilers;
        }

        private void OnEnable()
        {
            SelectSpoiler(_carConfig.CurrentSpoiler);
        }

        public void SelectSpoiler(int id)
        {
            _selectedID = id;
            StartCoroutine(_scroll.FocusAtPointCoroutine(_spoilers[id].transform.localPosition, 1.5f));

            _car.SelectSpoiler(_selectedID);

            _propertyField.Find("Name").Find("Logo").GetComponent<Image>().sprite = _carConfig.CurrentCar.SpoilersIconsSprites[id];
            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _carConfig.CurrentCar.SpoilersNames[id];
            _currentCost = _carConfig.CurrentCar.SpoilersCost[id];
            _propertyField.Find("Cost").GetComponent<TextMeshProUGUI>().text = _currentCost.ToString(CustomStringFormat.CoinFormat(_currentCost)) + " <sprite index=1>";

            bool hasSpoiler = false;
            foreach (int spoiler in _openedSpoilers)
            {
                if (spoiler == id)
                {
                    hasSpoiler = true;
                    break;
                }
            }

            if (_carConfig.CurrentSpoiler == _selectedID)
                _actionButton.ButtonSetActive(ActionButtonType.Selected);
            else
            {
                if (hasSpoiler)
                {
                    _actionButton.ButtonSetActive(ActionButtonType.Apply);
                    _actionButton.UpdateAction(() => ApplySelectedSpoiler());
                }
                else
                {
                    _actionButton.ButtonSetActive(ActionButtonType.Buy);
                    _actionButton.UpdateAction(() => BuySelectedSpoiler());
                }
            }
        }

        public void BuySelectedSpoiler()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _openedSpoilers.Add(_selectedID);
                _actionButton.ButtonSetActive(ActionButtonType.Apply);
                _actionButton.UpdateAction(() => ApplySelectedSpoiler());
                DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ApplySelectedSpoiler()
        {
            _car.SetSpoiler(_selectedID);
            _actionButton.ButtonSetActive(ActionButtonType.Selected);
            if (_carConfig.CostsDictionary.ContainsKey("Spoiler"))
                _carConfig.CostsDictionary.Remove("Spoiler");
            _carConfig.CostsDictionary.Add("Spoiler", _currentCost);
            _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
            _carInfo.UpdateDisplayData();
        }
    }
}