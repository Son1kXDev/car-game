using System.Net;
using UnityEngine;
using UnityEngine.Localization.Custom;
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
        [SerializeField] private PopupPanel _settingsPanel, _pausePanel;
        private PopupBackground _popupBackground;
        private Game.CameraController _cameraController;
        private bool _wasPaused;

        private void Awake()
        {
            if (Instance) Destroy(this);
            else Instance = this;

            _cameraController = FindObjectOfType<Game.CameraController>();
        }

        private void Start()
        {
            GlobalEventManager.Instance.OnPauseButtonPressed += DisplayPausePopup;
            GlobalEventManager.Instance.OnSettingsButtonPressed += DisplaySettingsPopup;
            GlobalEventManager.Instance.OnGarageMenuButtonPressed += DisplayGarageMenuExitConfirmation;
            GlobalEventManager.Instance.OnGetReward += DisplayReward;
            GlobalEventManager.Instance.OnFinishTheLevel += DisplayFinish;
        }

        private void OnDestroy()
        {
            GlobalEventManager.Instance.OnPauseButtonPressed -= DisplayPausePopup;
            GlobalEventManager.Instance.OnSettingsButtonPressed -= DisplaySettingsPopup;
            GlobalEventManager.Instance.OnGarageMenuButtonPressed -= DisplayGarageMenuExitConfirmation;
            GlobalEventManager.Instance.OnGetReward -= DisplayReward;
            GlobalEventManager.Instance.OnFinishTheLevel -= DisplayFinish;
        }

        public void ButtonMainMenu()
        {
            Data.DataPersistenceManager.Instance.SaveGame();
            SceneLoadManager.Instance.LoadScene("MainMenuScene");
        }

        public void ButtonExit() => Application.Quit();

        public void ButtonNewgame()
        {
            if (Data.DataPersistenceManager.Instance.SaveFileExist())
            {
                string displayText = Localization.GetCurrentLanguage() == Lang.English ?
                "Are you sure you want to overwrite the current save? All progress made will be lost." :
                "Вы уверены что хотите перезаписать текущее сохранение? Весь прогресс будет потерян.";
                _confirmationPopup.ActivatePopup(displayText,
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

        public void DisplayReward(int rewardValue, string rewardLable)
        {
            string rewardValueText = rewardValue.ToString(CustomStringFormat.CoinFormat(rewardValue));
            _rewardData.text = $"+{rewardValueText} <sprite index=4>";
            _rewardPanel.SetRewardData(rewardLable);
            _rewardPanel.gameObject.SetActive(true);
        }

        public void DisplayFinish()
        {
            Game.FinishTrigger finish = FindObjectOfType<Game.FinishTrigger>();

            Data.DataPersistenceManager.Instance.SaveGame();

            string displayText = Localization.GetCurrentLanguage() == Lang.English ?
            "Congratulations! You have finished this map!" : "Поздравляем! Вы прошли карту!";

            string yesButtonText = Localization.GetCurrentLanguage() == Lang.English ?
            "Back to menu" : "Назад в меню";

            string noButtonText = Localization.GetCurrentLanguage() == Lang.English ?
            "Continue" : "Продолжить";

            _confirmationPopup.ActivatePopup(displayText,
            () => SceneLoadManager.Instance.LoadScene("GameMenuScene"),
            () =>
            {
                finish.OnContinue();
                _cameraController.LockInput(false);
            },
            yesButtonText, noButtonText);
        }

        public void DisplayUpdatePopup(string downloadURL)
        {
            _popupBackground.gameObject.SetActive(true);
            string displayText = Localization.GetCurrentLanguage() == Lang.English ?
            "New update available! Click \"Download\" to install it now!" :
            "Новое обновление доступно! Нажмите \"Скачать\" чтобы установить его сейчас!";

            string yesButtonText = Localization.GetCurrentLanguage() == Lang.English ?
            "Download" : "Скачать";

            string noButtonText = Localization.GetCurrentLanguage() == Lang.English ?
            "Remind me later" : "Напомнить позже";

            _confirmationPopup.ActivatePopup(displayText,
            () =>
            {
                Application.OpenURL(downloadURL);
                Application.Quit();
            },
            () => Debug.Log("Update canceled"),
            yesButtonText, noButtonText);
        }

        public void DisplayPausePopup(bool enabled)
        {
            if (enabled) _pausePanel.gameObject.SetActive(true);
            else _pausePanel.Disable();

            ButtonSound(true);
        }

        public void DisplaySettingsPopup(bool enabled)
        {
            if (enabled)
            {
                if (_pausePanel != null && _pausePanel.gameObject.activeSelf == true)
                {
                    _wasPaused = true;
                    _pausePanel.Disable();
                }
                _settingsPanel.gameObject.SetActive(true);
            }
            else if (_wasPaused)
            {
                _settingsPanel.Disable();
                _pausePanel.gameObject.SetActive(true);
                _wasPaused = false;
            }
            else _settingsPanel.Disable();
            ButtonSound(true);
        }

        public void DisplayGarageMenuExitConfirmation(bool enabled)
        {
            _pausePanel.Disable();
            ButtonSound(true);
            Data.DataPersistenceManager.Instance.SaveGame();
            string displayText = Localization.GetCurrentLanguage() == Lang.English ?
            "Are you sure you want to exit to menu?" : "Вы уверены что хотите выйти в меню?";
            _confirmationPopup.ActivatePopup(displayText,
            () => SceneLoadManager.Instance.LoadScene("GameMenuScene"),
            () => _pausePanel.gameObject.SetActive(true));
        }

        public void ButtonSound(bool value) =>
        AudioManager.Instance.PlayOneShot(value ? Audio.Data.ButtonClick : Audio.Data.ButtonNoClick, transform.position);

    }
}