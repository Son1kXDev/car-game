using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CoinManager : MonoBehaviour, Data.IDataPersistence
    {
        public static CoinManager Instance;

        private int _coins;
        private int newCoinsValue;
        private bool _changeCoinValue = false;
        private Color _coinColor = new(255, 238, 123, 255);

        [SerializeField] private Color _mainColor = new(255, 238, 123, 255);
        [SerializeField] private Color _increaseColor = new(255, 238, 123, 255);
        [SerializeField] private Color _decreaseColor = new(255, 238, 123, 255);

        private void Awake()
        {
            if (Instance) Destroy(gameObject);
            else Instance = this;
        }

        public void LoadData(GameData data)
        {
            _coins = data.Coins;
            _coinColor = _mainColor;
            UI.UIManager.Instance.DisplayCoins(_coins.ToString(), _coinColor);
        }

        public void SaveData(GameData data)
        {
            data.Coins = _coins;
        }

        public bool DecreaseCoins(int value)
        {
            if (_coins < value) return false;

            _coinColor = _decreaseColor;
            newCoinsValue = _coins - value;
            _changeCoinValue = true;
            return true;
        }

        private void Update()
        {
            if (!_changeCoinValue) return;

            _coins = (int)Mathf.MoveTowards(_coins, newCoinsValue, 1550 * Time.deltaTime);
            UI.UIManager.Instance.DisplayCoins(_coins.ToString(), _coinColor);
            if (_coins == newCoinsValue)
            {
                _changeCoinValue = false;
                _coinColor = _mainColor;
                UI.UIManager.Instance.DisplayCoins(_coins.ToString(), _coinColor);
            }
        }

        public void IncreaseCoins(int value)
        {
            _changeCoinValue = true;
            newCoinsValue = _coins + value;
            _coinColor = _increaseColor;
        }
    }
}