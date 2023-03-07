using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.UI
{
    public class GarageCar : MonoBehaviour, Data.IDataPersistence
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _isOpened = false;
        private CarConfig _config;

        public void LoadData(GameData data)
        {
            _config = GetComponent<CarConfig>();

            data.carsOpened.TryGetValue(_id, out _isOpened);
        }

        public void SaveData(GameData data)
        {
            if (data.carsOpened.ContainsKey(_id))
                data.carsOpened.Remove(_id);

            data.carsOpened.Add(_id, _isOpened);
        }

        public void BuyCar()
        {
            if (_isOpened) return;

            if (Game.CoinManager.Instance.DecreaseCoins((int)_config.FindVisualsConfigByID(_id).Cost))
            {
                _isOpened = true;
                Data.DataPersistenceManager.Instance.SaveGame();
            }
        }
    }
}