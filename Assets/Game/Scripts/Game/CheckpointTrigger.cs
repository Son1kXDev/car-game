using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Localization.Custom;
using DG.Tweening;
using FMODUnity;

namespace Assets.Game.Scripts.Game
{

    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(StudioEventEmitter))]
    public class CheckpointTrigger : MonoBehaviour
    {
        [SerializeField] private LocalizedString _rewardLabel;
        [SerializeField] private int _rewardValue = 1000;
        private SpriteRenderer _renderer;
        private bool _checkpoint = false;
        private Tween _tween;
        private Light2D[] _lights;
        private StudioEventEmitter _eventEmitter;

        private void OnEnable()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _lights = GetComponentsInChildren<Light2D>(false);

            _tween = DOTween.Sequence()
            .Append(_renderer.DOFade(0.45f, 1.5f))
            .AppendInterval(0.1f)
            .Append(_renderer.DOFade(0.75f, 1.5f))
            .AppendInterval(0.1f)
            .SetLoops(-1)
            .SetLink(gameObject);
        }

        private void Start()
        {
            _eventEmitter = AudioManager.Instance.InitializeEventEmitter(Audio.Data.CheckpointIdle, this.gameObject);
            _eventEmitter.Play();
        }

        private void OnCheckpoint()
        {
            _checkpoint = true;

            AudioManager.Instance.PlayOneShot(Audio.Data.Checkpoint, transform.position);

            string rewardLabel = Localization.GetCurrentLanguage() == Lang.English ? _rewardLabel.EN : _rewardLabel.RU;
            GlobalEventManager.Instance.GetReward(_rewardValue, rewardLabel);

            _tween.Kill();

            DOTween.To(() => _lights[0].intensity, x => _lights[0].intensity = x, 0, 0.5f).SetLink(gameObject);
            DOTween.To(() => _lights[1].intensity, x => _lights[1].intensity = x, 0, 0.5f).SetLink(gameObject);
            _renderer.DOFade(0, 0.5f)
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                _eventEmitter.Stop();
                gameObject.SetActive(false);
            });

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player" && _checkpoint == false)
                OnCheckpoint();
        }
    }
}