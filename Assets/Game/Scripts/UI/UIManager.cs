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
        [SerializeField] private TextMeshProUGUI _rewardData;
        [SerializeField] private ConfirmationPopup _confirmationPopup;
        [SerializeField] private RewardPanel _rewardPanel;
        [SerializeField] private SettingsPanel _settingsPanel;

        private Game.CameraController _cameraController;

        private void Awake()
        {
            if (Instance) Destroy(this);
            else Instance = this;

            _cameraController = FindObjectOfType<Game.CameraController>();
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
            () => _settingsPanel.gameObject.SetActive(true));
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
            Data.DataPersistenceManager.Instance.ResetGame();
            Data.DataPersistenceManager.Instance.NewGame();
            PlayerPrefs.DeleteKey("UpgradeMenuPosition");
            FindObjectOfType<ContinueButton>().Start();
            SceneLoadManager.Instance.LoadScene("GameMenuScene");
        }

        public void ButtonContinue()
        {
            Data.DataPersistenceManager.Instance.LoadGame();
            SceneLoadManager.Instance.LoadScene("GameMenuScene");
        }

        public void Play(int ID)
        {
            Data.DataPersistenceManager.Instance.SaveGame();
            SceneLoadManager.Instance.LoadScene(ID);
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

        public void DisplayCoins(string value, Color color, int spriteIndex = 0)
        {
            string sprite = $"<sprite index={spriteIndex}>";
            _coinData.ForEach(text => { text.text = $"{value} {sprite}"; });
            _coinData.ForEach(text => { text.color = color; });
        }

        public void DisplayReward(string rewardLable, string rewardValue)
        {
            _rewardData.text = $"+{rewardValue} <sprite index=4>";
            _rewardPanel.SetRewardData(rewardLable);
            _rewardPanel.gameObject.SetActive(true);
        }

        public void DisplayFinish()
        {
            Game.FinishTrigger finish = FindObjectOfType<Game.FinishTrigger>();

            _confirmationPopup.ActivatePopup("Congratulations! You have finished this map!",
            () => SceneLoadManager.Instance.LoadScene("GameMenuScene"),
            () =>
            {
                finish.OnContinue();
                _cameraController.LockInput(false);
            },
            "Back to menu",
            "Continue");
        }

        public void ButtonSound(bool value) =>
        AudioManager.Instance.PlayOneShot(value ? Audio.Data.ButtonClick : Audio.Data.ButtonNoClick, transform.position);

    }
}