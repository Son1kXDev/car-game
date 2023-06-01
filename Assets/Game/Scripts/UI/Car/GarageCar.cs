using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.UI
{
    public class GarageCar : MonoBehaviour, Data.IDataPersistence
    {
        [SerializeField, StatusIcon] private string _id;
        [SerializeField] private bool _isOpened = false;
        private CarConfig _config;

        public void LoadData(GameData data)
        {
            _config = GetComponent<CarConfig>();

            data.CarsOpened.TryGetValue(_id, out _isOpened);
        }

        public void SaveData(GameData data)
        {
            if (data.CarsOpened.ContainsKey(_id))
                data.CarsOpened.Remove(_id);

            data.CarsOpened.Add(_id, _isOpened);
        }

        public void BuyCar()
        {
            if (_isOpened) return;

            if (Game.CoinManager.Instance.DecreaseCoins((int)_config.FindCarConfigByID(_id).Cost))
            {
                _isOpened = true;
                Data.DataPersistenceManager.Instance.SaveGame();
            }
        }
    }
}