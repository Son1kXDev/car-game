using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts
{
    public class CarConfig : MonoBehaviour, Data.IDataPersistence
    {
        public Car CurrentCar;
        [HideInInspector] public Color CurrentColor;
        [HideInInspector] public int CurrentTire;
        [HideInInspector] public int CurrentRim;
        [HideInInspector] public int CurrentSpoiler;
        [HideInInspector] public int CurrentSplitter;
        [HideInInspector] public string CurrentStickerPath;
        [HideInInspector] public Upgrades CurrentCarUpgrades;
        [HideInInspector] public List<int> Costs;
        [HideInInspector] public SerializableDictionary<string, int> CostsDictionary;

        [SerializeField] private List<Car> _carsConfigs;

        public bool HasConfig()
        { return CurrentCar != null; }


        public void LoadData(GameData data)
        {
            string id = data.CurrentCar;
            ColorUtility.TryParseHtmlString(data.CarsColors[id], out CurrentColor);
            CurrentTire = data.CurrentTires;
            CurrentRim = data.CurrentRims;
            CurrentSpoiler = data.CurrentSpoiler;
            CurrentSplitter = data.CurrentSplitter;
            CurrentCarUpgrades = data.CarUpgrades[id];
            CurrentStickerPath = data.CurrentStickerPath;

            CurrentCar = _carsConfigs.Find(car => car.ID == id);
            CostsDictionary = data.Costs;
            Costs = new List<int>(CostsDictionary.Values);
        }

        public void SaveData(GameData data)
        {
            data.CarsColors[data.CurrentCar] = "#" + ColorUtility.ToHtmlStringRGBA(CurrentColor);
            data.CurrentTires = CurrentTire;
            data.CurrentRims = CurrentRim;
            data.CurrentSpoiler = CurrentSpoiler;
            data.CurrentSplitter = CurrentSplitter;
            data.CarUpgrades[data.CurrentCar] = CurrentCarUpgrades;
            data.Costs = CostsDictionary;
            data.CurrentStickerPath = CurrentStickerPath;
        }

        public Car FindCarConfigByID(string id)
        { return _carsConfigs.Find(car => car.ID == id); }
    }
}