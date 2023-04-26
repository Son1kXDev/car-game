using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SettingsPanel : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void OnEnable()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
        }

        public void Disable()
        {
            _canvasGroup.DOFade(0, 0.5f)
            .SetLink(gameObject)
            .OnKill(() => gameObject.SetActive(false));
        }

    }
}