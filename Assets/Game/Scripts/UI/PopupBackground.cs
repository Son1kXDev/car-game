using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopupBackground : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        private void Start() => GlobalEventManager.Instance.OnPauseButtonPressed += SetActive;
        private void OnDestroy() => GlobalEventManager.Instance.OnPauseButtonPressed -= SetActive;


        private void SetActive(bool active)
        {
            _canvasGroup.DOFade(active && _canvasGroup.alpha == 0 ? 1 : 0, 0.5f).SetLink(gameObject);
        }
    }
}