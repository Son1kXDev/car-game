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
        [SerializeField] private Transform _propertyField;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private List<GameObject> _splitters;

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

            _propertyField.Find("Name").Find("Logo").GetComponent<Image>().sprite = _carConfig.VisualCarConfig.SplittersIconsSprites[id];
            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _carConfig.VisualCarConfig.SplittersNames[id];
            _currentCost = _carConfig.VisualCarConfig.SplittersCost[id];
            _propertyField.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"{_currentCost} <sprite index=1>";

            bool hasSplitter = false;
            foreach (int splitter in _openedSplitters)
            {
                if (splitter == id)
                {
                    hasSplitter = true;
                    break;
                }
            }

            Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
            if (_carConfig.CurrentSplitter == _selectedID)
            {
                actionButton.interactable = false;
                actionButton.GetComponent<Image>().color = new(255, 255, 255, 0);
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Selected";
            }
            else
            {
                actionButton.interactable = true;
                actionButton.GetComponent<Image>().color = new(255, 255, 255, 255);
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = hasSplitter ? "Apply" : "Buy";
                actionButton.onClick.RemoveAllListeners();
                if (hasSplitter) actionButton.onClick.AddListener(() => ApplySelectedSplitter());
                else actionButton.onClick.AddListener(() => BuySelectedSplitter());
            }
        }

        public void BuySelectedSplitter()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _openedSplitters.Add(_selectedID);
                Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Apply";
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(() => ApplySelectedSplitter());
                DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ApplySelectedSplitter()
        {
            _car.SetSplitter(_selectedID);
            UI.UIManager.Instance.ButtonSound(true);
            Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
            actionButton.interactable = false;
            actionButton.GetComponent<Image>().color = new(255, 255, 255, 0);
            actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Selected";
            if (_carConfig.CostsDictionary.ContainsKey("Splitter"))
                _carConfig.CostsDictionary.Remove("Splitter");
            _carConfig.CostsDictionary.Add("Splitter", _currentCost);
            _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
            _carInfo.UpdateDisplayData();
        }
    }
}