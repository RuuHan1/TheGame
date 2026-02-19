using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class LevelUpText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _moveDistance = 2f;
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private Color _targetColor = Color.white;

    void Start()
    {
        transform.DOMoveY(transform.position.y + _moveDistance, _duration)
            .SetEase(Ease.OutQuad);

        _text.DOColor(_targetColor, _duration * 0.5f);

        _text.DOFade(0, _duration)
            .SetEase(Ease.InExpo)
            .OnComplete(() => LeanPool.Despawn(this.gameObject));
    }
}
