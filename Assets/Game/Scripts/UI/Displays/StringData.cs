using UnityEngine;
using System.Collections;
using TMPro;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class StringData : MonoBehaviour
    {
        [SerializeField] private Type _type;
        private enum Type { Speedometer, Tachometer, Gearbox };
        private TextMeshProUGUI _text;

        private void Awake() => _text = GetComponent<TextMeshProUGUI>();

        private void Start()
        {
            switch (_type)
            {
                case Type.Speedometer:
                    GlobalEventManager.Instance.OnSpeedometerDataChanged += DisplayData;
                    break;
                case Type.Tachometer:
                    GlobalEventManager.Instance.OnTachometerDataChanged += DisplayData;
                    break;
                case Type.Gearbox:
                    GlobalEventManager.Instance.OnGearboxDataChanged += DisplayData;
                    break;
            }
        }

        private void OnDisable()
        {
            switch (_type)
            {
                case Type.Speedometer:
                    GlobalEventManager.Instance.OnSpeedometerDataChanged -= DisplayData;
                    break;
                case Type.Tachometer:
                    GlobalEventManager.Instance.OnTachometerDataChanged -= DisplayData;
                    break;
                case Type.Gearbox:
                    GlobalEventManager.Instance.OnGearboxDataChanged -= DisplayData;
                    break;
            }
        }

        public void DisplayData(string value) => _text.text = value;

    }
}