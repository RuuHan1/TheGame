using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whell : MonoBehaviour
{
    [SerializeField] private List<Transform> _iconPoints = new();
    //[SerializeField] private List<Sprite> _wheelIcons = new();
    [SerializeField] private List<WheelStuff> _wheelStufs = new();
    private Dictionary<CardType, Sprite> _wheelIcons = new();
    private SpriteRenderer[] _renderers;
    private void Start()
    {
        foreach (var s in _wheelStufs)
            _wheelIcons.Add(s._tag, s._icon);

        _renderers = new SpriteRenderer[_iconPoints.Count];
        for (int i = 0; i < _iconPoints.Count; i++)
        {
            // Yoksa ekle
            _renderers[i] = _iconPoints[i].GetComponent<SpriteRenderer>()
                         ?? _iconPoints[i].gameObject.AddComponent<SpriteRenderer>();

            _renderers[i].sprite = GetRandomSprite();
        }
    }
    public Tween SpinTween; // Dýþarýdan durdurmak için

    public void StartSpin(CardType target, float duration)
    {
        StartCoroutine(SpinRoutine(target, duration));
    }
    private IEnumerator SpinRoutine(CardType target, float duration)
    {
        float elapsed = 0f;
        float interval = 0.08f;

        while (elapsed < duration)
        {
            yield return ScrollOneStep(interval);

            elapsed += interval;
            interval = Mathf.Lerp(0.08f, 0.35f, elapsed / duration); // Yavaþla
        }

        // Son adýmda hedef ikonu ortaya (index 1) yerleþtir
        _renderers[1].sprite = _wheelIcons[target];
    }

    private IEnumerator ScrollOneStep(float duration)
    {
        // Tüm noktalarý bir sonrakine doðru tween et
        for (int i = 0; i < _iconPoints.Count; i++)
        {
            int nextIndex = (i + 1) % _iconPoints.Count;
            _renderers[i].transform
                .DOMove(_iconPoints[nextIndex].position, duration)
                .SetEase(Ease.Linear);
        }

        yield return new WaitForSeconds(duration);

        // Pozisyonlarý sýfýrla, sprite'larý kaydýr
        Sprite topSprite = _renderers[0].sprite;
        for (int i = 0; i < _renderers.Length - 1; i++)
        {
            _renderers[i].sprite = _renderers[i + 1].sprite;
            _renderers[i].transform.position = _iconPoints[i].position;
        }
        // En sona yeni random sprite
        _renderers[^1].sprite = GetRandomSprite();
        _renderers[^1].transform.position = _iconPoints[^1].position;
    }

    private Sprite GetRandomSprite()
    {
        var keys = new List<CardType>(_wheelIcons.Keys);
        return _wheelIcons[keys[UnityEngine.Random.Range(0, keys.Count)]];
    }

    [Serializable]
    private struct WheelStuff
    {
        public CardType _tag;
        public Sprite _icon;
    }
}

