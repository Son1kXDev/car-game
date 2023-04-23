using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts
{
    public class CarConfig : MonoBehaviour, Data.IDataPersistence
    {
        public Car MainCarConfig;
        public CarVisualConfig VisualCarConfig;
        [HideInInspector] public Color CurrentColor;
        [HideInInspector] public int CurrentTire;
        [HideInInspector] public int CurrentRim;
        [HideInInspector] public int CurrentSpoiler;
        [HideInInspector] public int CurrentSplitter;
        [HideInInspector] public string CurrentStickerPath;
        [HideInInspector] public Upgrades CurrentCarUpgrades;
        [HideInInspector] public List<int> Costs;
        [HideInInspector] public SerializableDictionary<string, int> CostsDictionary;

        [SerializeField] private List<Car> _mainCarsConfigs;
        [SerializeField] private List<CarVisualConfig> _visualCarsConfigs;

        public bool HasConfig()
        {
            return MainCarConfig != null && VisualCarConfig != null;
        }

        public bool RightID()
        {
            return MainCarConfig.ID == VisualCarConfig.ID;
        }

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

            MainCarConfig = _mainCarsConfigs.Find(car => car.ID == id);
            VisualCarConfig = _visualCarsConfigs.Find(car => car.ID == id);
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

        public CarVisualConfig FindVisualsConfigByID(string id)
        {
            return _visualCarsConfigs.Find(car => car.ID == id);
        }

        public Car FindMainCarConfigByID(string id)
        {
            return _mainCarsConfigs.Find(car => car.ID == id);
        }
    }
}