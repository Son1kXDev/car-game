using UnityEngine;
using System.Collections;
using TMPro;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CoinData : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake() => _text = GetComponent<TextMeshProUGUI>();

        private void OnEnable()
        {
            StopAllCoroutines();
            GlobalEventManager.Instance.OnCoinDataChanged += DisplayCoins;
            if (Game.CoinManager.Instance == null) StartCoroutine(GetCurrentCoinData());
            else Game.CoinManager.Instance.GetCurrentCoinData();
        }

        private IEnumerator GetCurrentCoinData()
        {
            yield return null;
            Game.CoinManager.Instance.GetCurrentCoinData();
        }

        private void OnDisable() => GlobalEventManager.Instance.OnCoinDataChanged -= DisplayCoins;

        public void DisplayCoins(string value, Color color, int spriteIndex = 0)
        {
            string sprite = $"<sprite index={spriteIndex}>";
            _text.text = $"{value} {sprite}";
            _text.color = color;
        }
    }
}