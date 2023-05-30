using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalEventManager : MonoBehaviour
{
    public static GlobalEventManager Instance { get; private set; }

    #region Events
    public event BoolEvent OnGasButtonPressed;
    public event BoolEvent OnBrakeButtonPressed;
    public event BoolEvent OnGearButtonPressed;
    public event BoolEvent OnLightButtonPressed;
    public event BoolEvent OnPauseButtonPressed;
    public event BoolEvent OnSettingsButtonPressed;
    public event BoolEvent OnGarageMenuButtonPressed;
    public event IntStringEvent OnGetReward;
    public event ActionEvent OnConfirmationPopupCalled;
    public event EmptyEvent OnFinishTheLevel;
    public event EmptyEvent OnParticleToggleChanged;
    public event EmptyEvent OnLanguageChanged;
    public event CoinEvent OnCoinDataChanged;
    public event StringEvent OnSpeedometerDataChanged;
    public event StringEvent OnTachometerDataChanged;
    public event StringEvent OnGearboxDataChanged;
    #endregion
    #region Delegates
    public delegate void BoolEvent(bool value);
    public delegate void IntStringEvent(int value, string label);
    public delegate void StringEvent(string value);
    public delegate void CoinEvent(string value, Color color, int spriteIndex);
    public delegate void IntEvent(int value);
    public delegate void ActionEvent(string title, UnityAction confirmAction, UnityAction cancelAction);
    public delegate void EmptyEvent();
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this);
    }

    public void GasButton(bool value) => OnGasButtonPressed?.Invoke(value);
    public void BrakeButton(bool value) => OnBrakeButtonPressed?.Invoke(value);
    public void GearButton(bool value) => OnGearButtonPressed?.Invoke(value);
    public void LightButton(bool value) => OnLightButtonPressed?.Invoke(value);
    public void PauseButton(bool value) => OnPauseButtonPressed?.Invoke(value);
    public void SettingsButton(bool value) => OnSettingsButtonPressed?.Invoke(value);
    public void GarageMenuButton(bool value) => OnGarageMenuButtonPressed?.Invoke(value);
    public void GetReward(int reward, string label) => OnGetReward(reward, label);
    public void CoinDataChanged(string value, Color color, int spriteIndex = 0) => OnCoinDataChanged?.Invoke(value, color, spriteIndex);
    public void SpeedometerDataChanged(string value) => OnSpeedometerDataChanged?.Invoke(value);
    public void TachometerDataChanged(string value) => OnTachometerDataChanged?.Invoke(value);
    public void GearboxDataChanged(string value) => OnGearboxDataChanged?.Invoke(value);
    public void FinishLevel() => OnFinishTheLevel?.Invoke();
    public void LanguageChanged() => OnLanguageChanged?.Invoke();
    public void ParticleToggleChanged() => OnParticleToggleChanged?.Invoke();
    public void ActivateConfirmationPopup(string displayLabel, UnityAction confirmAction, UnityAction cancelAction)
    => OnConfirmationPopupCalled?.Invoke(displayLabel, confirmAction, cancelAction);

}
