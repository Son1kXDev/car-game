using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

namespace Assets.Game.Scripts.UI
{
    [Component("Car Info", "Displays current car config visual information")]
    public class CarInfo : MonoBehaviour
    {
        [SerializeField, StatusIcon] private TextMeshProUGUI _name;
        [SerializeField, StatusIcon] private TextMeshProUGUI _weight;
        [SerializeField, StatusIcon] private TextMeshProUGUI _strength;
        [SerializeField, StatusIcon] private TextMeshProUGUI _costText;
        [SerializeField] private LocalizedString _localCost;
        [SerializeField] private LocalizedString _localWeight;
        [SerializeField] private LocalizedString _localStrength;

        private Coroutine _updateCostData;
        private int _cost = 0;
        private CarConfig _carConfig;

        private IEnumerator Start()
        {
            while (_carConfig == null)
            {
                _carConfig = FindObjectOfType<CarConfig>();
                yield return null;
            }
            _localCost.Arguments = new object[] { _cost.ToString(@"###\.###") };
            _localCost.StringChanged += UpdateCostData;
            _localWeight.Arguments = new object[] { _carConfig.CurrentCar.Weight.ToString() };
            _localWeight.StringChanged += UpdateWeightData;
            _localStrength.Arguments = new object[] { _carConfig.CurrentCar.Strength.ToString() };
            _localStrength.StringChanged += UpdateStrengthData;
            SceneManager.activeSceneChanged += SceneChanged;
            yield return new WaitForEndOfFrame();
            UpdateDisplayData(true);
        }

        private void SceneChanged(Scene oldScene, Scene newScene)
        {
            _localCost.StringChanged -= UpdateCostData;
            _localWeight.StringChanged -= UpdateWeightData;
            _localStrength.StringChanged -= UpdateStrengthData;
        }

        public void UpdateDisplayData(bool instant = false)
        {
            _name.text = _carConfig.CurrentCar.Name;
            _localWeight.Arguments[0] = _carConfig.CurrentCar.Weight.ToString();
            _localStrength.Arguments[0] = _carConfig.CurrentCar.Strength.ToString();
            _localWeight.RefreshString();
            _localStrength.RefreshString();
            int cashCost = _cost;
            _cost = (int)_carConfig.CurrentCar.Cost;
            int newCost = _cost;
            if (cashCost != 0) _cost = cashCost;
            _carConfig.Costs.ForEach(c => newCost += c);
            float speed = instant ? float.MaxValue : Mathf.Abs(_cost - newCost) * 4;
            if (_updateCostData != null) StopCoroutine(_updateCostData);
            _updateCostData = StartCoroutine(UpdateCostData(newCost, speed));
        }

        private void UpdateCostData(string value) => _costText.text = value;
        private void UpdateWeightData(string value) => _weight.text = value;
        private void UpdateStrengthData(string value) => _strength.text = value;

        private IEnumerator UpdateCostData(int newCost, float speed)
        {
            int previousCost = _cost;
            _cost = newCost;
            while (previousCost != newCost)
            {
                previousCost = (int)Mathf.MoveTowards(previousCost, newCost, speed * Time.deltaTime);
                _localCost.Arguments[0] = previousCost.ToString(@"###\.###");
                _localCost.RefreshString();
                yield return null;
            }
            _localCost.Arguments[0] = _cost.ToString(@"###\.###");
            _localCost.RefreshString();
        }
    }
}