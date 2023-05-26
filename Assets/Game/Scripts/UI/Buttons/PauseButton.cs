using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private bool _enable;
        private Button _button;
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => GlobalEventManager.Instance.PauseButton(_enable));
        }
    }
}
