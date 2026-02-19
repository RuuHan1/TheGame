using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;

    private Sequence _sequence;

    public void Initialize(float damageAmount,bool isCritical)
    {
        damageText.text = Mathf.RoundToInt(damageAmount).ToString();
        if (isCritical)
        {
            damageText.color = Color.red;
            damageText.fontSize = 10;
        }
        else
        {
            damageText.color = Color.white;
            damageText.fontSize = 8;
        }
        PlayAnimation();
    }
    private void PlayAnimation()
    {
        _sequence?.Kill();
        transform.localScale = Vector3.one;
        damageText.alpha = 1f;
        _sequence = DOTween.Sequence();
        Vector3 targetPos = transform.position + Vector3.up * 1.5f;
        _sequence.Append(transform.DOMove(targetPos, 0.5f).SetEase(Ease.OutBack));
        _sequence.Join(damageText.DOFade(0, 0.5f).SetEase(Ease.InQuad));
        _sequence.OnComplete(() =>
        {
            LeanPool.Despawn(this.gameObject);
        });
    }
    private void OnDisable()
    {
        _sequence?.Kill();
    }
    
}
