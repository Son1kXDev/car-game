using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CoinManager : MonoBehaviour, Data.IDataPersistence
    {
        public static CoinManager Instance;

        [SerializeField] private Color _mainColor = new(255, 238, 123, 255);
        [SerializeField] private Color _increaseColor = new(255, 238, 123, 255);
        [SerializeField] private Color _decreaseColor = new(255, 238, 123, 255);
        [SerializeField] private Color _noMoneyColor = Color.red;
        private int _coins;
        private int _newCoinsValue;
        private int _maxValue = 999999999;
        private float _speed;
        private bool _changeCoinValue = false;
        private bool _decrease = false;
        private Color _coinColor = new(255, 238, 123, 255);

        private void Awake()
        {
            if (Instance) Destroy(gameObject);
            else Instance = this;
        }

        public void LoadData(GameData data)
        {
            _coins = data.Coins;
            _coinColor = _mainColor;
            UI.UIManager.Instance.DisplayCoins(_coins.ToString(CustomStringFormat.CoinFormat(_coins)), _coinColor);
        }

        public void SaveData(GameData data)
        {
            data.Coins = _coins;
        }

        public bool DecreaseCoins(int value)
        {
            bool success = _coins >= value;
            if (success)
            {
                _speed = value * 4;
                _coinColor = _decreaseColor;
                _newCoinsValue = _coins - value;
                _changeCoinValue = true;
                _decrease = true;
                AudioManager.Instance.PlayOneShot(Audio.Data.Purchase, transform.position);
            }
            else
            {
                UI.UIManager.Instance.ButtonSound(success);
                UI.UIManager.Instance.DisplayCoins(_coins.ToString(CustomStringFormat.CoinFormat(_coins)), _noMoneyColor, 2);
                StartCoroutine(ResetColorByDelay(0.2f));
            }
            return success;
        }

        private void Update()
        {
            if (!_changeCoinValue) return;

            _coins = (int)Mathf.MoveTowards(_coins, _newCoinsValue, _speed * Time.deltaTime);
            UI.UIManager.Instance.DisplayCoins(_coins.ToString(CustomStringFormat.CoinFormat(_coins)), _coinColor, _decrease ? 3 : 4);
            if (_coins == _newCoinsValue)
            {
                _changeCoinValue = false;
                _coinColor = _mainColor;
                UI.UIManager.Instance.DisplayCoins(_coins.ToString(CustomStringFormat.CoinFormat(_coins)), _coinColor);
            }
        }

        //TODO: Fix increase display
        public void IncreaseCoins(int value)
        {
            AudioManager.Instance.PlayOneShot(Audio.Data.GetMoney, transform.position);
            _changeCoinValue = true;
            _decrease = false;
            _speed = value * 2f;
            _newCoinsValue = _coins + value;
            if (_newCoinsValue > _maxValue)
            {
                _newCoinsValue = _maxValue;
                _changeCoinValue = false;
                return;
            }
            _coinColor = _increaseColor;
        }

        IEnumerator ResetColorByDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UI.UIManager.Instance.DisplayCoins(_coins.ToString(CustomStringFormat.CoinFormat(_coins)), _coinColor);
        }
    }
}

public static class CustomStringFormat
{
    public static string CoinFormat(float value)
    {
        if (value > 999999999) return @"###\.###\.###\.###";
        else if (value > 999999) return @"###\.###\.###";
        else if (value > 999) return @"###\.###";
        else return "##0";
    }
}