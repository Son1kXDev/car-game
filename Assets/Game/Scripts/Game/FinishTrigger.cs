using UnityEngine;
using DG.Tweening;

namespace Assets.Game.Scripts.Game
{

    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class FinishTrigger : MonoBehaviour
    {
        [SerializeField] private string _rewardLable;
        [SerializeField] private int _rewardValue = 1000;
        private SpriteRenderer _renderer;
        private Material _material;
        private bool _finish = false;

        private void OnEnable()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _material = _renderer.material;

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

            DOTween.Sequence()
            .AppendInterval(3f)
            .AppendCallback(() => UI.UIManager.Instance.DisplayFinish())
            .SetLink(gameObject)
            .OnKill(() =>
            {
                CameraController controller = FindObjectOfType<CameraController>(true);
                controller.LockInput(true);
            });

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player" && _finish == false)
                OnFinish();
        }
    }
}