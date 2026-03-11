using DG.Tweening;
using UnityEngine;

public class XpMover : MonoBehaviour
{
    [SerializeField] private float bounceDistance = 0.5f;
    [SerializeField] private float bounceTime = 0.15f;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float acceleration = 10f;

    private Transform _target;
    private bool _isFollowing;
    private float _currentSpeed;

    public void MoveToPlayer(Transform player)
    {
        transform.DOKill(); // önceki tween varsa temizle
        _target = player;
        _isFollowing = false;
        _currentSpeed = followSpeed;

        Vector3 awayDir = (transform.position - player.position).normalized;
        Vector3 bounceTarget = transform.position + awayDir * bounceDistance;

        transform.DOMove(bounceTarget, bounceTime)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => _isFollowing = true);
    }

    private void Update()
    {
        if (!_isFollowing || _target == null) return;

        _currentSpeed += acceleration * Time.deltaTime;

        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.position,
            _currentSpeed * Time.deltaTime
        );
    }

    private void OnDisable()
    {
        transform.DOKill();
        _isFollowing = false;
        _currentSpeed = 0;
    }
}