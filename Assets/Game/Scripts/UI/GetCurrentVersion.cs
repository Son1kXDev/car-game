using UnityEngine;
using TMPro;
using Utils.Debugger;

namespace Assets.Game.Scripts.UI
{
    public class GetCurrentVersion : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _versionDataText;

        private void Awake()
        {
            string version = Application.version;
            Console.Log(version);
            _versionDataText.text = _versionDataText.text.Replace("{data}", version);
        }
    }
}