
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopupPanel : MonoBehaviour
    {
        [SerializeField] private bool _includeParticles = false;
        [SerializeField] private List<ParticleSystem> _particles = new List<ParticleSystem>();
        private CanvasGroup _canvasGroup;

        private void OnEnable()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            if (_includeParticles) _particles.ForEach(p =>
            { p.gameObject.SetActive(true); p.enableEmission = ParticleController.Particles; });

            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
        }

        public void Disable()
        {
            Data.DataPersistenceManager.Instance.LoadSettings();

            DOTween.Sequence()
            .AppendCallback(() => _particles.ForEach(p => p.gameObject.SetActive(false)))
                .Append(_canvasGroup.DOFade(0.0f, 0.5f))
                .SetLink(gameObject)
                .OnKill(() => gameObject.SetActive(false));
        }

    }
}