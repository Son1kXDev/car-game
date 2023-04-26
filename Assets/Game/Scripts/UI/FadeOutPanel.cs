using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class FadeOutPanel : MonoBehaviour
{
    [SerializeField] private float _delay = 0.1f;
    private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 1f;
        DOTween.Sequence()
        .AppendInterval(_delay)
        .Append(_canvasGroup.DOFade(0, 1f))
        .AppendCallback(() => Destroy(this.gameObject));
    }
}
