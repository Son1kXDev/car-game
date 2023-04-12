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
            //MenuCameraController.Instance.SetCameraPosition(new(2.36f, -1.34f, -10));
            // MenuCameraController.Instance.SetCameraSize(0.9f);
        }

        public void SelectTire(int id)
        {
            _selectedID = id;
            StartCoroutine(_scroll.FocusAtPointCoroutine(_tires[id].transform.localPosition, 1.5f));

            _car.SelectTire(_selectedID);

            _propertyField.Find("Logo").GetComponent<Image>().sprite = _carConfig.VisualCarConfig.TiresSprites[id];
            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _carConfig.VisualCarConfig.TiresNames[id];
            _currentCost = _carConfig.VisualCarConfig.TiresCost[id];
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

            Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
            if (_carConfig.CurrentTire == _selectedID)
            {
                actionButton.interactable = false;
                actionButton.GetComponent<Image>().color = new(255, 255, 255, 0);
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Selected";
            }
            else
            {
                actionButton.interactable = true;
                actionButton.GetComponent<Image>().color = new(255, 255, 255, 255);
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = hasTire ? "Apply" : "Buy";
                actionButton.onClick.RemoveAllListeners();
                if (hasTire) actionButton.onClick.AddListener(() => ApplySelectedTire());
                else actionButton.onClick.AddListener(() => BuySelectedTire());
            }
        }

        public void BuySelectedTire()
        {
            if (CoinManager.Instance.DecreaseCoins(_currentCost))
            {
                _openedTires.Add(_selectedID);
                ApplySelectedTire();
                Button actionButton = _propertyField.Find("ActionButton").GetComponent<Button>();
                actionButton.transform.Find("Lable").GetComponent<TextMeshProUGUI>().text = "Apply";
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(() => ApplySelectedTire());
                DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ApplySelectedTire()
        {
            _car.SetTire(_selectedID);
            if (_carConfig.CostsDictionary.ContainsKey("Tire"))
                _carConfig.CostsDictionary.Remove("Tire");
            _carConfig.CostsDictionary.Add("Tire", _currentCost);
            _carConfig.Costs = new(_carConfig.CostsDictionary.Values);
            _carInfo.UpdateDisplayData();
        }
    }
}