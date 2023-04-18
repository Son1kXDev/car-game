using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Assets.Game.Scripts.Game
{
    public class CoinManager : MonoBehaviour, Data.IDataPersistence
    {
        public static CoinManager Instance;

        [SerializeField] private Color _mainColor = new(255, 238, 123, 255);
        [SerializeField] private Color _increaseColor = new(255, 238, 123, 255);
        [SerializeField] private Color _decreaseColor = new(255, 238, 123, 255);
        [SerializeField] private Color _noMoneyColor = Color.red;
        [SerializeField] EventReference _purchaseSound;

        private int _coins;
        private int newCoinsValue;
        private float _decreaseSpeed;
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
            UI.UIManager.Instance.DisplayCoins(_coins.ToString(), _coinColor);
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
                _decreaseSpeed = value * 4;
                _coinColor = _decreaseColor;
                newCoinsValue = _coins - value;
                _changeCoinValue = true;
                _decrease = true;
                AudioManager.Instance.PlayOneShot(_purchaseSound, transform.position);
            }
            else
            {
                UI.UIManager.Instance.ButtonSound(success);
                UI.UIManager.Instance.DisplayCoins(_coins.ToString(), _noMoneyColor, 2);
                StartCoroutine(ResetColorByDelay(0.2f));
            }
            return success;
        }

        private void Update()
        {
            if (!_changeCoinValue) return;

            _coins = (int)Mathf.MoveTowards(_coins, newCoinsValue, _decreaseSpeed * Time.deltaTime);
            UI.UIManager.Instance.DisplayCoins(_coins.ToString(), _coinColor, _decrease ? 3 : 4);
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
            _decrease = false;
            newCoinsValue = _coins + value;
            _coinColor = _increaseColor;
        }

        IEnumerator ResetColorByDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UI.UIManager.Instance.DisplayCoins(_coins.ToString(), _coinColor);
        }
    }
}