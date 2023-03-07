using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CoinManager : MonoBehaviour, Data.IDataPersistence
    {
        public static CoinManager Instance;

        private int _coins;

        private void Awake()
        {
            if (Instance) Destroy(gameObject);
            else Instance = this;
        }

        public void LoadData(GameData data)
        {
            _coins = data.Coins;
            UI.UIManager.Instance.DisplayCoins(_coins.ToString());
        }

        public void SaveData(GameData data)
        {
            data.Coins = _coins;
        }

        public bool DecreaseCoins(int value)
        {
            if (_coins < value) return false;

            _coins -= value;
            UI.UIManager.Instance.DisplayCoins(_coins.ToString());
            return true;
        }

        public void IncreaseCoins(int value)
        {
            _coins += value;
            UI.UIManager.Instance.DisplayCoins(_coins.ToString());
        }
    }
}