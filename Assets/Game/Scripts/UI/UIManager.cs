using UnityEngine;
using TMPro;
using Utils.Debugger;

namespace Assets.Game.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private TextMeshProUGUI _speedometerData;
        [SerializeField] private TextMeshProUGUI _gearboxData;
        [SerializeField] private TextMeshProUGUI _tachometerData;
        [SerializeField] private TextMeshProUGUI _coinData;

        private void Awake()
        {
            if (Instance) Destroy(this);
            else Instance = this;
        }

        public void ButtonMenu()
        {
            SceneLoadManager.Instance.LoadScene("MainMenuScene");
        }

        public void ButtonExit() => Application.Quit();

        public void ButtonNewgame()
        {
            Data.DataPersistenceManager.Instance.NewGame();
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
            FindObjectOfType<ContinueButton>().Start();
            FindObjectOfType<ResetButton>().Start();
        }

        public void ButtonPlay()
        {
            //todo play button
        }

        public void ButtonAchievements()
        {
        }

        public void ButtonShop()
        {
        }

        public void ButtonUpgrades()
        {
        }

        public void ButtonInventory()
        {
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

        public void DisplayCoins(string value)
        {
            if (_coinData != null)
                _coinData.text = value;
        }
    }
}