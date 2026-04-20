using UnityEngine;
using Lean.Pool;
using System.Collections.Generic;
using System.Collections;
using System;
using DG.Tweening;
public class Weapon : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float range = 5f;
    [SerializeField] private Transform bulletSpawnPoint;

    private float _fire = 0f;
    //suanlik deaktif
    [SerializeField] private WeaponSO _currentWeaponSO;
    [SerializeField] private List<CardSO> _cardsInSlots = new List<CardSO>();
    private bool _isFiring = false;
    [Header("Events")]
    [SerializeField] private WeaponChangedEvent weaponChangedEvent;
    [SerializeField] private WeaponState weaponState;
    private WeaponInstance _weaponInstance;

    private LineRenderer _lineRenderer;
    [SerializeField] private int segments = 60;
    private bool _isRangeActive = false;
    private Collider2D[] _enemyBuffer = new Collider2D[100];
    private ContactFilter2D _contactFilter;

    private void OnEnable()
    {
        GameEvents.WeaponSlotChanged += OnWeaponSlotChanged;
        GameEvents.WeaponSlotCountChanged += OnWeaponSlotAmountChanged;
        GameEvents.ActivateWeaponRange_PlayerHud += ToggleRangeCircle;
    }



    private void OnDisable()
    {
        GameEvents.WeaponSlotChanged -= OnWeaponSlotChanged;
        GameEvents.WeaponSlotCountChanged -= OnWeaponSlotAmountChanged;
        GameEvents.ActivateWeaponRange_PlayerHud -= ToggleRangeCircle;
    }
    private void Awake()
    {
        if (_currentWeaponSO != null)
        {
            _weaponInstance = new WeaponInstance(_currentWeaponSO);
        }
        _contactFilter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = enemyLayer,
            //useTriggers = true  // trigger collider kullanýyorsan
        };
        _lineRenderer = GetComponentInParent<LineRenderer>();

    }
    private void Start()
    {
        _lineRenderer.enabled = _isRangeActive;
        if (_weaponInstance != null)
        {
            GameEvents.CardAwarded?.Invoke(_weaponInstance.DefaultProjectile);
            ApplyCards();
            SetWeapon(_weaponInstance.TotalSlots);
        }
    }

    private void OnWeaponSlotChanged(List<CardViewSO> list)
    {
        _cardsInSlots.Clear();
        var temp = new List<CardViewSO>(list);
        foreach (CardViewSO card in temp)
        {
            _cardsInSlots.Add(card.CardData);
        }


    }

    private void Update()
    {
        if (Time.time < _fire) return;
        if (_weaponInstance.Containers == null) return;
        CheckDistance();
    }
    private void CheckDistance()
    {
        if (_isFiring) return;

        int count = Physics2D.OverlapCircle(
               transform.position, range, _contactFilter, _enemyBuffer);

        if (count == 0) return;

#if UNITY_EDITOR
    if (count == _enemyBuffer.Length)
        Debug.LogWarning($"{name}: Enemy buffer dolu, boyutu artýr!");
    DrawRangeCircle(range);
#endif

        Transform closestEnemy = null;
        float minDistanceSqr = float.MaxValue; // Mathf.Infinity yerine daha temiz
        Vector3 currentPosition = transform.position;

        for (int i = 0; i < count; i++)
        {
            float dSqr = (_enemyBuffer[i].transform.position - currentPosition).sqrMagnitude;
            if (dSqr < minDistanceSqr)
            {
                minDistanceSqr = dSqr;
                closestEnemy = _enemyBuffer[i].transform;
            }
        }

        if (closestEnemy != null)
            StartCoroutine(Fire(closestEnemy)); // pozisyon deðil Transform
    }


    public IEnumerator Fire(Transform enemyTransform)
    {
        Vector2 pos =   new Vector2(enemyTransform.position.x,enemyTransform.position.y);
        _isFiring = true;

        var containers = _weaponInstance.Containers;
        var triggers = _weaponInstance.triggerContainers;
        float multiCastSpreadAngle = 10f;
        for (int i = 0; i < containers.Count;)
        {
            int batchCount = Mathf.Max(1, _weaponInstance.MultiCastCount);
            int remaining = containers.Count - i;
            batchCount = Mathf.Min(batchCount, remaining);

            float totalSpread = multiCastSpreadAngle * (batchCount - 1);
            float startAngle = -totalSpread / 2f;

            for (int j = 0; j < batchCount; j++)
            {
                var container = containers[i + j];

                if (container.ProjectilePrefab == null)
                    continue;

                if (container.IsTriggered)
                {
                    triggers.Add(container);
                    continue;
                }

                if (triggers != null && triggers.Count > 0)
                {
                    container.OnHitPayloads = new List<ProjectileContainer>(triggers);
                }

                float angleOffset = startAngle + (multiCastSpreadAngle * j);

                SpawnBullet(container, pos, angleOffset);
            }

            float delay = Mathf.Max(0.05f, containers[i].CastDelay);
            yield return new WaitForSeconds(delay);

            i += batchCount;
        }
        _weaponInstance.ResetWeapon();
        _isFiring = false;
        ApplyCards();
        _fire = Time.time + _weaponInstance.RechargeTime;
    }

    private void SpawnBullet(ProjectileContainer container, Vector2 targetPos, float angleOffset)
    {
        GameObject bullet = LeanPool.Spawn(container.ProjectilePrefab, bulletSpawnPoint.position, Quaternion.identity);
        var bulletComp = bullet.GetComponent<Projectile>();
        var rb = bullet.GetComponent<Rigidbody2D>();

        bulletComp.Initialize(container);

        Vector2 baseDirection = (targetPos - (Vector2)bulletSpawnPoint.position).normalized;

        // Açýyý hesapla
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        float finalAngle = baseAngle + angleOffset;

        Vector2 finalDirection = new Vector2(
            Mathf.Cos(finalAngle * Mathf.Deg2Rad),
            Mathf.Sin(finalAngle * Mathf.Deg2Rad));

        if (rb != null)
        {
            rb.linearVelocity = finalDirection * container.Speed;
            bullet.transform.rotation = Quaternion.AngleAxis(finalAngle, Vector3.forward);
        }
    }

    private void ApplyCards()
    {
        if (_weaponInstance.Containers == null)
        {
            _weaponInstance.Containers = new List<ProjectileContainer>();
        }

        _weaponInstance.Containers.Clear();

        List<CardSO> pendingCards = new List<CardSO>();

        foreach (var card in _cardsInSlots)
        {
            if (card.CardType == CardType.Modifier || card.CardType == CardType.Augment || card.CardType == CardType.Utility)
            {
                pendingCards.Add(card);
            }
            else if (card.CardType == CardType.Projectile)
            {
                ProjectileContainer newContainer = new ProjectileContainer();
                card.UpdateContainer(newContainer, _weaponInstance);
                foreach (var pendingCard in pendingCards)
                {
                    pendingCard.UpdateContainer(newContainer, _weaponInstance);
                }
                pendingCards.Clear();
                _weaponInstance.Containers.Add(newContainer);
            }

        }

    }
    public void OnWeaponSlotAmountChanged(int count)
    {
        _weaponInstance.BonusSlots += count;
        SetWeapon(_weaponInstance.TotalSlots);
    }
    public void SetWeapon(int slot)
    {

        weaponState.SetSlot(slot);
        weaponChangedEvent.RaiseEvent(slot);
    }
    public void DrawRangeCircle(float radius)
    {
        if (_lineRenderer == null && !_lineRenderer.enabled) return;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = segments;

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            _lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
    private void ToggleRangeCircle()
    {
        _isRangeActive = !_isRangeActive;
        _lineRenderer.enabled = _isRangeActive;
    }
}