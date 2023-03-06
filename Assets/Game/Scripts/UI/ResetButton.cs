using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class ResetButton : MonoBehaviour
    {
        [SerializeField] private Color _activeColor = Color.white;
        [SerializeField] private Color _inactiveColor = Color.gray;

        private Button _button;
        private TextMeshProUGUI _line;

        public void Start()
        {
            _button = GetComponent<Button>();
            _line = transform.Find("Line").GetComponent<TextMeshProUGUI>();
            bool SaveFile = Data.DataPersistenceManager.Instance.SaveFileExist();
            _button.interactable = SaveFile;
            _line.color = SaveFile ? _activeColor : _inactiveColor;
        }
    }
}