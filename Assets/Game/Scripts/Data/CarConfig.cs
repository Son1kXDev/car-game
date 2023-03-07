using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts
{
    public class CarConfig : MonoBehaviour, Data.IDataPersistence
    {
        [HideInInspector] public Car MainCarConfig;
        [HideInInspector] public CarVisualConfig VisualCarConfig;
        [HideInInspector] public Color CurrentColor;

        [SerializeField] private List<Car> _mainCarsConfigs;
        [SerializeField] private List<CarVisualConfig> _visualCarsConfigs;

        public bool HasConfig()
        {
            return MainCarConfig != null || VisualCarConfig != null;
        }

        public bool RightID()
        {
            return MainCarConfig.ID == VisualCarConfig.ID;
        }

        public void LoadData(GameData data)
        {
            string id = data.CurrentCar;
            ColorUtility.TryParseHtmlString(data.BaseColor, out CurrentColor);
            MainCarConfig = _mainCarsConfigs.Find(car => car.ID == id);
            VisualCarConfig = _visualCarsConfigs.Find(car => car.ID == id);
        }

        public void SaveData(GameData data)
        {
            data.BaseColor = "#" + ColorUtility.ToHtmlStringRGBA(CurrentColor);
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