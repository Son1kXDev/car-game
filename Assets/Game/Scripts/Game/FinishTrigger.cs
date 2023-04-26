using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace Assets.Game.Scripts.Game
{

    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class FinishTrigger : MonoBehaviour
    {
        [SerializeField] private string _rewardLable;
        [SerializeField] private int _rewardValue = 1000;
        private SpriteRenderer _renderer;
        private Light2D _light;
        private Material _material;
        private bool _finish = false;
        private Tween _tween;

        private void OnEnable()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _material = _renderer.material;
            _light = GetComponentInChildren<Light2D>();

            DOTween.Sequence()
            .Append(_material.DOOffset(Vector2.down, 2f).SetEase(Ease.Linear))
            .SetLoops(-1)
            .SetLink(gameObject);
        }

        private void OnFinish()
        {
            _finish = true;
            AudioManager.Instance.PlayOneShot(Audio.Data.Finish, transform.position);
            UI.UIManager.Instance.DisplayReward(_rewardLable, _rewardValue.ToString(CustomStringFormat.CoinFormat(_rewardValue)));

            DOTween.Sequence()
            .AppendInterval(2f)
            .AppendCallback(() => CoinManager.Instance.IncreaseCoins(_rewardValue));

            _tween = DOTween.Sequence()
            .AppendInterval(3f)
            .AppendCallback(() => UI.UIManager.Instance.DisplayFinish())
            .SetLink(gameObject)
            .OnKill(() =>
            {
                CameraController controller = FindObjectOfType<CameraController>(true);
                controller.LockInput(true);
            });

        }

        public void OnContinue()
        {
            _tween.Kill();
            DOTween.To(() => _light.intensity, x => _light.intensity = x, 0, 1f);

            DOTween.Sequence()
            .Append(_renderer.DOFade(0, 1f))
            .OnKill(() => gameObject.SetActive(false));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player" && _finish == false)
                OnFinish();
        }
    }
}