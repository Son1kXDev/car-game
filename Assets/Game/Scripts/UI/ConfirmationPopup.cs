using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class ConfirmationPopup : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        public void ActivatePopup(string displayText, UnityAction confirmAction, UnityAction cancelAction)
        {
            gameObject.SetActive(true);
            _displayText.text = displayText;
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();

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

        private void DeactivatePopup() => gameObject.SetActive(false);
    }
}