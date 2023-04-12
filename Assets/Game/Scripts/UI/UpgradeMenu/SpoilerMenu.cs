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
        [SerializeField] private Transform _propertyField;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private List<GameObject> _spoilers;

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

            _propertyField.Find("Logo").GetComponent<Image>().sprite = _carConfig.VisualCarConfig.SpoilersIconsSprites[id];
            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _carConfig.VisualCarConfig.SpoilersNames[id];
            _currentCost = _carConfig.VisualCarConfig.SpoilersCost[id];
            _propertyField.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"{_currentCost} <sprite index=1>";

            bool hasSpoiler = false;
            foreach (int spoiler in _openedSpoilers)
            {
                if (spoiler == id)
                {
                    hasSpoiler = true;
                    break;
                }
            }

            Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
            if (_carConfig.CurrentSpoiler == _selectedID)
            {
                actionButton.interactable = false;
                actionButton.GetComponent<Image>().color = new(255, 255, 255, 0);
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Selected";
            }
            else
            {
                actionButton.interactable = true;
                actionButton.GetComponent<Image>().color = new(255, 255, 255, 255);
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = hasSpoiler ? "Apply" : "Buy";
                actionButton.onClick.RemoveAllListeners();
                if (hasSpoiler) actionButton.onClick.AddListener(() => ApplySelectedSpoiler());
                else actionButton.onClick.AddListener(() => BuySelectedSpoiler());
            }
        }

        public void BuySelectedSpoiler()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _openedSpoilers.Add(_selectedID);
                ApplySelectedSpoiler();
                Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Apply";
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(() => ApplySelectedSpoiler());
                DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ApplySelectedSpoiler()
        {
            _car.SetSpoiler(_selectedID);
            if (_carConfig.CostsDictionary.ContainsKey("Spoiler"))
                _carConfig.CostsDictionary.Remove("Spoiler");
            _carConfig.CostsDictionary.Add("Spoiler", _currentCost);
            _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
            _carInfo.UpdateDisplayData();
        }
    }
}