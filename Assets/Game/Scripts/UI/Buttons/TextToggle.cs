using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Custom;
using TMPro;

namespace Assets.Game.Scripts.UI
{
    public class TextToggle : Toggle
    {
        private TextMeshProUGUI _text;

        protected override void Awake()
        {
            base.Awake();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            this.onValueChanged.AddListener(UpdateToggle);
            this.onValueChanged.AddListener((value) => UIManager.Instance.ButtonSound());
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateToggle(this.isOn);
        }

        private void UpdateToggle(bool value)
        {
            string ON = Localization.GetCurrentLanguage() == Lang.English ? "On" : "Вкл";
            string OFF = Localization.GetCurrentLanguage() == Lang.English ? "Off" : "Выкл";
            _text.text = value ? ON : OFF;
        }

    }
}
