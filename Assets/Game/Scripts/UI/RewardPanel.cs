using UnityEngine;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

namespace Assets.Game.Scripts.UI
{
    public class RewardPanel : MonoBehaviour
    {
        [SerializeField, StatusIcon] TextMeshProUGUI _rewardLable;

        private CanvasGroup _canvasGroup;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            _canvasGroup.DOFade(1, 1f);
            transform.DOMoveY(1.5f, 1f);

            DOTween.Sequence()
            .AppendInterval(3f)
            .Append(_canvasGroup.DOFade(0, 1f))
            .Append(transform.DOMoveY(0, 1f))
            .AppendCallback(() => gameObject.SetActive(false));
        }


        public void SetRewardData(string lable) => _rewardLable.text = lable;

    }
}