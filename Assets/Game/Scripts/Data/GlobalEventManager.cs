using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalEventManager : MonoBehaviour
{
    public static GlobalEventManager Instance { get; private set; }

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

    public delegate void BoolEvent(bool value);
    public delegate void IntStringEvent(int value, string label);

    public delegate void ActionEvent(string title, UnityAction confirmAction, UnityAction cancelAction);
    public delegate void EmptyEvent();

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

    public void FinishLevel() => OnFinishTheLevel?.Invoke();
    public void LanguageChanged() => OnLanguageChanged?.Invoke();
    public void ParticleToggleChanged() => OnParticleToggleChanged?.Invoke();

    public void ActivateConfirmationPopup(string displayLabel, UnityAction confirmAction, UnityAction cancelAction)
    => OnConfirmationPopupCalled?.Invoke(displayLabel, confirmAction, cancelAction);

}
