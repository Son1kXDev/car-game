using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CoinManager : MonoBehaviour, Data.IDataPersistence
    {
        private int _coins;

        public void LoadData(GameData data)
        {
            _coins = data.Coins;
            UI.UIManager.Instance.DisplayCoins(_coins.ToString());
        }

        public void SaveData(GameData data)
        {
            data.Coins = _coins;
        }

        public void DecreaseCoins(int value)
        {
            if (_coins < value) return;

            _coins -= value;
            UI.UIManager.Instance.DisplayCoins(_coins.ToString());
        }

        public void IncreaseCoins(int value)
        {
            _coins += value;
            UI.UIManager.Instance.DisplayCoins(_coins.ToString());
        }
    }
}