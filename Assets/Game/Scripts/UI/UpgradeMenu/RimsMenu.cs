using Assets.Game.Scripts.Data;
using Assets.Game.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class RimsMenu : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private Transform _propertyField;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private List<GameObject> _rims;

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
            SelectRim(0);
        }

        public void SelectRim(int id)
        {
            _selectedID = id;
            StartCoroutine(_scroll.FocusAtPointCoroutine(_rims[id].transform.localPosition, 1.5f));

            _propertyField.Find("Logo").GetComponent<Image>().sprite = _carConfig.VisualCarConfig.RimsSprites[id];
            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _carConfig.VisualCarConfig.RimsNames[id];
            _currentCost = _carConfig.VisualCarConfig.RimsCost[id];
            _propertyField.Find("Cost").GetComponent<TextMeshProUGUI>().text = $"{_currentCost} <sprite index=1>";

            bool hasRim = false;
            foreach (int rims in _openedRims)
            {
                if (rims == id)
                {
                    hasRim = true;
                    break;
                }
            }

            Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
            if (_carConfig.CurrentRim == _selectedID)
            {
                actionButton.interactable = false;
                actionButton.GetComponent<Image>().color = new(255, 255, 255, 0);
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Selected";
            }
            else
            {
                actionButton.interactable = true;
                actionButton.GetComponent<Image>().color = new(255, 255, 255, 255);
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = hasRim ? "Apply" : "Buy";
                actionButton.onClick.RemoveAllListeners();
                if (hasRim) actionButton.onClick.AddListener(() => ApplySelectedRim());
                else actionButton.onClick.AddListener(() => BuySelectedRim());
            }
        }

        public void BuySelectedRim()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _openedRims.Add(_selectedID);
                ApplySelectedRim();
                Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Apply";
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(() => ApplySelectedRim());
                DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ApplySelectedRim()
        {
            _car.SetRim(_selectedID);
            if (_carConfig.CostsDictionary.ContainsKey("Rim"))
                _carConfig.CostsDictionary.Remove("Rim");
            _carConfig.CostsDictionary.Add("Rim", _currentCost);
            _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
            _carInfo.UpdateDisplayData();
        }
    }
}