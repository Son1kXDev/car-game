using Assets.Game.Scripts.Data;
using Assets.Game.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class SplitterMenu : MonoBehaviour, IDataPersistence
    {
        [SerializeField, StatusIcon(offset: 20)] private Transform _propertyField;
        [SerializeField, StatusIcon(offset: 20)] private ScrollRect _scroll;
        [SerializeField] private List<GameObject> _splitters;
        [SerializeField, StatusIcon(offset: 20)] private ActionButton _actionButton;

        private CarConfig _carConfig;
        private CarInfo _carInfo;
        private MenuCar _car;

        private List<int> _openedSplitters;

        private int _currentCost;
        private int _selectedID;

        public void LoadData(GameData data)
        {
            _carConfig = FindFirstObjectByType<CarConfig>();
            _car = FindFirstObjectByType<MenuCar>();
            _carInfo = FindObjectOfType<CarInfo>(true);
            _openedSplitters = data.OpenedSplitters;
        }

        public void SaveData(GameData data)
        {
            data.OpenedSplitters = _openedSplitters;
        }


        private void OnEnable()
        {
            SelectSplitter(_carConfig.CurrentSplitter);
        }

        public void SelectSplitter(int id)
        {
            _selectedID = id;
            StartCoroutine(_scroll.FocusAtPointCoroutine(_splitters[id].transform.localPosition, 1.5f));

            _car.SelectSplitter(_selectedID);

            _propertyField.Find("Name").Find("Logo").GetComponent<Image>().sprite = _carConfig.CurrentCar.SplittersIconsSprites[id];
            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _carConfig.CurrentCar.SplittersNames[id];
            _currentCost = _carConfig.CurrentCar.SplittersCost[id];
            _propertyField.Find("Cost").GetComponent<TextMeshProUGUI>().text = _currentCost.ToString(CustomStringFormat.CoinFormat(_currentCost)) + " <sprite index=1>";

            bool hasSplitter = false;
            foreach (int splitter in _openedSplitters)
            {
                if (splitter == id)
                {
                    hasSplitter = true;
                    break;
                }
            }

            if (_carConfig.CurrentSplitter == _selectedID)
                _actionButton.ButtonSetActive(ActionButtonType.Selected);
            else
            {
                if (hasSplitter)
                {
                    _actionButton.ButtonSetActive(ActionButtonType.Apply);
                    _actionButton.UpdateAction(() => ApplySelectedSplitter());
                }
                else
                {
                    _actionButton.ButtonSetActive(ActionButtonType.Buy);
                    _actionButton.UpdateAction(() => BuySelectedSplitter());
                }
            }
        }

        public void BuySelectedSplitter()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _openedSplitters.Add(_selectedID);
                _actionButton.ButtonSetActive(ActionButtonType.Apply);
                _actionButton.UpdateAction(() => ApplySelectedSplitter());
                DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ApplySelectedSplitter()
        {
            _car.SetSplitter(_selectedID);
            _actionButton.ButtonSetActive(ActionButtonType.Selected);
            if (_carConfig.CostsDictionary.ContainsKey("Splitter"))
                _carConfig.CostsDictionary.Remove("Splitter");
            _carConfig.CostsDictionary.Add("Splitter", _currentCost);
            _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
            _carInfo.UpdateDisplayData();
        }
    }
}