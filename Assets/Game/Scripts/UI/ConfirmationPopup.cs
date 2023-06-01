using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Localization.Custom;
using DG.Tweening;

namespace Assets.Game.Scripts.UI
{
    public class ConfirmationPopup : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField, StatusIcon(offset: 20)] private TextMeshProUGUI _displayText;
        [SerializeField, StatusIcon(offset: 20)] private Button _confirmButton;
        [SerializeField, StatusIcon(offset: 20)] private Button _cancelButton;

        private CanvasGroup _mainCanvasGroup;
        private Image _mainImage;
        private UnityAction _currentAction;

        private void Start()
        {
            GlobalEventManager.Instance.OnConfirmationPopupCalled += ActivatePopup;
            _mainImage = GetComponent<Image>();
            _mainCanvasGroup = GetComponent<CanvasGroup>();
            _mainImage.raycastTarget = false;
            _mainCanvasGroup.alpha = 0;
            _mainCanvasGroup.interactable = false;
        }
        private void OnDestroy() => GlobalEventManager.Instance.OnConfirmationPopupCalled -= ActivatePopup;

        public void ActivatePopup(string displayText, UnityAction confirmAction, UnityAction cancelAction)
        {
            string confirmButtonText = Localization.GetCurrentLanguage() == Lang.English ? "Yes" : "Да";
            string cancelButtonText = Localization.GetCurrentLanguage() == Lang.English ? "No" : "Нет";

            _mainCanvasGroup.interactable = true;
            _mainImage.raycastTarget = true;
            gameObject.SetActive(true);
            _mainCanvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
            _displayText.text = displayText;
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();

            _confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = confirmButtonText;
            _cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = cancelButtonText;

            _confirmButton.onClick.AddListener(() =>
            {
                _currentAction = confirmAction;
                UIManager.Instance.ButtonSound();
                DeactivatePopup();
            });

            _cancelButton.onClick.AddListener(() =>
            {
                _currentAction = cancelAction;
                UIManager.Instance.ButtonSound();
                DeactivatePopup();
            });
        }

        private void DeactivatePopup()
        {
            _currentAction?.Invoke();
            _mainCanvasGroup.interactable = false;
            _mainImage.raycastTarget = false;
            _mainCanvasGroup.DOFade(0, 0.5f)
            .SetLink(gameObject)
            .OnKill(() => gameObject.SetActive(false));
        }

    }
}