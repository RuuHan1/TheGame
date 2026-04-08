using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Whell : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _iconPoints = new();
    [SerializeField] private List<WheelStuff> _wheelStufs = new();

    private Dictionary<CardType, Sprite> _wheelIcons = new();
    private List<CardType> _cachedKeys = new();
    private Image[] _images;
    private Vector2[] _originalPositions;

    private bool _isStopping = false;
    private CardType _targetCard;

    private void Awake()
    {
        foreach (var s in _wheelStufs)
        {
            if (!_wheelIcons.ContainsKey(s._tag))
            {
                _wheelIcons.Add(s._tag, s._icon);
                _cachedKeys.Add(s._tag);
            }
        }

        int pointCount = _iconPoints.Count;
        _images = new Image[pointCount];
        _originalPositions = new Vector2[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            _images[i] = _iconPoints[i].GetComponent<Image>()
                         ?? _iconPoints[i].gameObject.AddComponent<Image>();

            _images[i].preserveAspect = true;
            _originalPositions[i] = _iconPoints[i].anchoredPosition;
            _images[i].sprite = GetRandomSprite();
        }
    }

    public void Spin()
    {
        _isStopping = false;
        StopAllCoroutines();
        StartCoroutine(SpinRoutine());
    }

    public void StopAt(CardType target)
    {
        _targetCard = target;
        _isStopping = true;
    }

    private IEnumerator SpinRoutine()
    {
        float currentInterval = 0.08f;

        // Dur komutu gelene kadar rastgele dön
        while (!_isStopping)
        {
            yield return StartCoroutine(ScrollOneStep(currentInterval, GetRandomSprite()));
        }

        // Durma sekansý: Yavaþla
        currentInterval = 0.2f;
        yield return StartCoroutine(ScrollOneStep(currentInterval, GetRandomSprite()));

        // Hedef ikonu en üste (index 0) sok
        currentInterval = 0.35f;
        yield return StartCoroutine(ScrollOneStep(currentInterval, _wheelIcons[_targetCard]));

        // Çark mekanik olarak burada biter, hedef ikon tam ortada (index 1) yer alýr.
    }

    private IEnumerator ScrollOneStep(float stepDuration, Sprite newTopSprite)
    {
        Tween lastTween = null;

        for (int i = 0; i < _iconPoints.Count; i++)
        {
            int nextIndex = (i + 1) % _iconPoints.Count;
            lastTween = _images[i].rectTransform
                .DOAnchorPos(_originalPositions[nextIndex], stepDuration)
                .SetEase(Ease.Linear).SetUpdate(true);
        }

        if (lastTween != null)
        {
            yield return lastTween.WaitForCompletion();
        }

        for (int i = _images.Length - 1; i > 0; i--)
        {
            _images[i].sprite = _images[i - 1].sprite;
        }

        // Dýþarýdan belirlenen sýradaki resmi en üste ata
        _images[0].sprite = newTopSprite;

        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].rectTransform.anchoredPosition = _originalPositions[i];
        }
    }

    private Sprite GetRandomSprite()
    {
        if (_cachedKeys.Count == 0) return null;
        return _wheelIcons[_cachedKeys[UnityEngine.Random.Range(0, _cachedKeys.Count)]];
    }

    [Serializable]
    private struct WheelStuff
    {
        public CardType _tag;
        public Sprite _icon;
    }

}