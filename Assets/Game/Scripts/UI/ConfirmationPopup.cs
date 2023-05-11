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
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void ActivatePopup(string displayText, UnityAction confirmAction, UnityAction cancelAction,
        string confirmButtonText = "1", string cancelButtonText = "0")
        {

            if (confirmButtonText == "1")
                confirmButtonText = Localization.GetCurrentLanguage() == Lang.English ? "Yes" : "Да";

            if (cancelButtonText == "0")
                cancelButtonText = Localization.GetCurrentLanguage() == Lang.English ? "No" : "Нет";

            _canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            _canvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
            _displayText.text = displayText;
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();

            _confirmButton.GetComponent<TextMeshProUGUI>().text = confirmButtonText;
            _cancelButton.GetComponent<TextMeshProUGUI>().text = cancelButtonText;

            _confirmButton.onClick.AddListener(() =>
            {
                DeactivatePopup();
                confirmAction();
            });

            _cancelButton.onClick.AddListener(() =>
            {
                DeactivatePopup();
                cancelAction();
            });
        }

        private void DeactivatePopup()
        {
            _canvasGroup.DOFade(0, 0.5f)
            .SetLink(gameObject)
            .OnKill(() => gameObject.SetActive(false));
        }

    }
}