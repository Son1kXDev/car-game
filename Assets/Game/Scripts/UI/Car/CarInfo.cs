using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.UI
{
    public class CarInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _weight;
        [SerializeField] private TextMeshProUGUI _strength;
        [SerializeField] private TextMeshProUGUI _costText;

        private Coroutine _updateCostData;

        private int _cost = 0;

        private CarConfig _carConfig;

        private void Start()
        {
            _carConfig = FindObjectOfType<CarConfig>();
            UpdateDisplayData(true);
        }

        public void UpdateDisplayData(bool instant = false)
        {
            _name.text = _carConfig.VisualCarConfig.Name;
            _weight.text = _carConfig.VisualCarConfig.Weight.ToString() + " kg";
            _strength.text = _carConfig.VisualCarConfig.Strength.ToString() + " HP";
            int cashCost = _cost;
            _cost = (int)_carConfig.VisualCarConfig.Cost;
            int newCost = _cost;
            if (cashCost != 0) _cost = cashCost;
            _carConfig.Costs.ForEach(c => newCost += c);
            float speed = instant ? float.MaxValue : Mathf.Abs(_cost - newCost) * 4;
            if (_updateCostData != null) StopCoroutine(_updateCostData);
            _updateCostData = StartCoroutine(UpdateCostData(newCost, speed));
        }

        private IEnumerator UpdateCostData(int newCost, float speed)
        {
            int previousCost = _cost;
            _cost = newCost;
            while (previousCost != newCost)
            {
                previousCost = (int)Mathf.MoveTowards(previousCost, newCost, speed * Time.deltaTime);
                _costText.text = $"Cost {previousCost.ToString(@"###\.###")} <sprite index=1>";
                yield return null;
            }
        }
    }
}