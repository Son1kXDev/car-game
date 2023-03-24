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
        [HideInInspector] public Upgrades CurrentCarUpgrades;

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
            CurrentCarUpgrades = data.CarUpgrades[id];

            MainCarConfig = _mainCarsConfigs.Find(car => car.ID == id);
            VisualCarConfig = _visualCarsConfigs.Find(car => car.ID == id);
        }

        public void SaveData(GameData data)
        {
            data.CarsColors[data.CurrentCar] = "#" + ColorUtility.ToHtmlStringRGBA(CurrentColor);
            data.CurrentTires = CurrentTire;
            data.CurrentRims = CurrentRim;
            data.CarUpgrades[data.CurrentCar] = CurrentCarUpgrades;
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