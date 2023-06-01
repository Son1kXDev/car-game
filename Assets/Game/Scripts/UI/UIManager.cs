using System;
using UnityEngine;
using UnityEngine.Localization.Custom;
using TMPro;

namespace Assets.Game.Scripts.UI
{
    public enum UIType { UI0Menu0Manager = 0, UI0Game0Manager = 1, UI0Manager = 2 }

    [Component("User Interface Manager")]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private UIType _type;
        [SerializeField, StatusIcon] private TextMeshProUGUI _rewardData;
        [SerializeField, StatusIcon] private RewardPanel _rewardPanel;
        [SerializeField, StatusIcon] private PopupPanel _settingsPanel, _pausePanel;
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
        #region BUTTONS
        public void ButtonExit() => Application.Quit();

        public void ButtonNewgame()
        {
            if (Data.DataPersistenceManager.Instance.SaveFileExist())
            {
                string displayText = Localization.GetCurrentLanguage() == Lang.English ?
                "Are you sure you want to overwrite the current save? All progress made will be lost." :
                "Вы уверены что хотите перезаписать текущее сохранение? Весь прогресс будет потерян.";

                GlobalEventManager.Instance.ActivateConfirmationPopup(displayText,
                    () => ButtonLoadNewGame(),
                    () => Debug.Log("Cancel overwriting savefile"));
            }
            else ButtonLoadNewGame();
        }

        public void ButtonLoadNewGame()
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

        public void ButtonPlay(int ID)
        {
            Data.DataPersistenceManager.Instance.SaveGame();
            SceneLoadManager.Instance.LoadScene(ID);
        }

        public void ButtonSound(bool value = true) =>
        AudioManager.Instance.PlayOneShot(value ? Audio.Data.ButtonClick : Audio.Data.ButtonNoClick, transform.position);
        #endregion
        #region DISPLAY
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
            "Congratulations! You have finished this map! \n Do you want to exit back to menu?"
            : "Поздравляем! Вы прошли карту! Хотите выйти назад в меню?";

            GlobalEventManager.Instance.ActivateConfirmationPopup(displayText,
            () => SceneLoadManager.Instance.LoadScene("GameMenuScene"),
            () =>
            {
                finish.OnContinue();
                _cameraController.LockInput(false);
            });
        }

        public void DisplayUpdatePopup(string downloadURL)
        {
            string displayText = Localization.GetCurrentLanguage() == Lang.English ?
            "New update available! Download it now?" :
            "Новое обновление доступно! Скачать его сейчас?";

            GlobalEventManager.Instance.ActivateConfirmationPopup(displayText,
            () =>
            {
                Application.OpenURL(downloadURL);
                Application.Quit();
            },
            () => Debug.Log("Update canceled"));
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
            ButtonSound();
        }

        public void DisplayGarageMenuExitConfirmation(bool enabled)
        {
            _pausePanel.Disable();
            ButtonSound();
            Data.DataPersistenceManager.Instance.SaveGame();
            string displayText = Localization.GetCurrentLanguage() == Lang.English ?
            "Are you sure you want to exit to menu?" : "Вы уверены что хотите выйти в меню?";
            GlobalEventManager.Instance.ActivateConfirmationPopup(displayText,
            () => SceneLoadManager.Instance.LoadScene("GameMenuScene"),
            () => _pausePanel.gameObject.SetActive(true));
        }
        #endregion

    }
}