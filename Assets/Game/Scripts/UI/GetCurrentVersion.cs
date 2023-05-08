using UnityEngine;
using UnityEngine.Localization;
using TMPro;
using Utils.Debugger;

namespace Assets.Game.Scripts.UI
{
    public class GetCurrentVersion : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _versionDataText;
        [SerializeField] private LocalizedString _localString;

        private string version;

        void OnEnable()
        {
            _localString.Arguments = new object[] { version };
            _localString.StringChanged += UpdateText;
        }

        private void Start()
        {
            Console.Log(version);
            version = Application.version;
            _localString.Arguments[0] = version;
            _localString.RefreshString();
        }

        private void OnDisable() => _localString.StringChanged -= UpdateText;

        private void UpdateText(string value) => _versionDataText.text = value;
    }
}