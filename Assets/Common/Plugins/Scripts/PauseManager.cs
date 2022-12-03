using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Pause
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager instance;

        public List<MonoBehaviour> monosToPause = new List<MonoBehaviour>();

        public PauseType Type = PauseType._switch;

        public Button PauseButton;

        public GameObject PausePanelPopup;

        private bool isPause = false;

        public bool showMonos = false;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(this);

            PauseAction(false);

            if (Type != PauseType._single) PauseButton.onClick.AddListener(PauseActionSwitch);
        }

        public void PauseAction(bool isPause)
        {
            this.isPause = isPause;
            foreach (var mono in monosToPause)
            {
                mono.enabled = !this.isPause;
            }
            Time.timeScale = this.isPause ? 0 : 1;
            if (Type == PauseType._switchWithPanel) PausePanelPopup.SetActive(isPause);
        }

        public void PauseActionSwitch()
        {
            isPause = !isPause;
            foreach (var mono in monosToPause)
            {
                mono.enabled = !isPause;
            }
            Time.timeScale = isPause ? 0 : 1;
            if (Type == PauseType._switchWithPanel) PausePanelPopup.SetActive(isPause);
        }
    }

    public enum PauseType
    {
        _single,
        _switch,
        _switchWithPanel
    }
}