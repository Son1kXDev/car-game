using UnityEngine;
using TMPro;
using Utils.Debugger;
using System.Collections.Generic;

namespace Assets.Game.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private TextMeshProUGUI _speedometerData;
        [SerializeField] private TextMeshProUGUI _gearboxData;
        [SerializeField] private TextMeshProUGUI _tachometerData;
        [SerializeField] private List<TextMeshProUGUI> _coinData;
        [SerializeField] private ConfirmationPopup _confirmationPopup;

        private void Awake()
        {
            if (Instance) Destroy(this);
            else Instance = this;
        }

        public void ButtonMainMenu()
        {
            Data.DataPersistenceManager.Instance.SaveGame();
            SceneLoadManager.Instance.LoadScene("MainMenuScene");
        }

        public void ButtonMenu()
        {
            Data.DataPersistenceManager.Instance.SaveGame();
            _confirmationPopup.ActivatePopup("Are you sure you want to exit to menu?",
            () => SceneLoadManager.Instance.LoadScene("GameMenuScene"),
            () => Debug.Log("Cancel exit to menu"));
        }

        public void ButtonExit() => Application.Quit();

        public void ButtonNewgame()
        {
            if (Data.DataPersistenceManager.Instance.SaveFileExist())
            {
                _confirmationPopup.ActivatePopup("Are you sure you want to overwrite the current save? All progress made will be lost.",
                    () => LoadNewGame(),
                    () => Debug.Log("Cancel overwriting savefile"));
            }
            else LoadNewGame();
        }

        public void LoadNewGame()
        {
            Data.DataPersistenceManager.Instance.NewGame();
            PlayerPrefs.DeleteAll();
            SceneLoadManager.Instance.LoadScene("GameMenuScene");
        }

        public void ButtonContinue()
        {
            Data.DataPersistenceManager.Instance.LoadGame();
            SceneLoadManager.Instance.LoadScene("GameMenuScene");
        }

        public void ButtonSettings()
        {
            //todo openSettingsPanel
        }

        public void ButtonReset()
        {
            Data.DataPersistenceManager.Instance.ResetGame();
            PlayerPrefs.DeleteAll();
            FindObjectOfType<ContinueButton>().Start();
            FindObjectOfType<ResetButton>().Start();
        }

        public void ButtonPlay()
        {
            Data.DataPersistenceManager.Instance.SaveGame();
            SceneLoadManager.Instance.LoadScene("GameScene");
        }

        public void DisplaySpeedometer(string value)
        {
            if (_speedometerData != null)
                _speedometerData.text = value;
        }

        public void DisplayGearbox(string value)
        {
            if (_gearboxData != null)
                _gearboxData.text = value;
        }

        public void DisplayTachometer(string value)
        {
            if (_tachometerData != null)
                _tachometerData.text = value;
        }

        public void DisplayCoins(string value, Color color)
        {
            _coinData.ForEach(text => { text.text = $"{value} <sprite index=0>"; });
            _coinData.ForEach(text => { text.color = color; });
        }
    }
}