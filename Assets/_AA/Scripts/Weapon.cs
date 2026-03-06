using UnityEngine;
using Lean.Pool;
using System.Collections.Generic;
using System.Collections;
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
    private void OnEnable()
    {
        GameEvents.WeaponSlotChanged += OnWeaponSlotChanged;
        GameEvents.WeaponSlotCountChanged += OnWeaponSlotAmountChanged;
    }
    private void OnDisable()
    {
        GameEvents.WeaponSlotChanged -= OnWeaponSlotChanged;
        GameEvents.WeaponSlotCountChanged -= OnWeaponSlotAmountChanged;
    }
    private void Awake()
    {
        if (_currentWeaponSO != null)
        {
            _weaponInstance = new WeaponInstance(_currentWeaponSO);
        }
        _lineRenderer = GetComponentInParent<LineRenderer>();
        if (_lineRenderer != null)
        {
            
            Debug.Log("LineRenderer component found in parent.");
        }
    }
    private void Start()
    {

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

        DrawRangeCircle(range);
        var enemy = Physics2D.OverlapCircle(transform.position, range, enemyLayer);
        if (enemy == null) return;
        Transform enemyObj = enemy.GetComponent<Transform>();
        StartCoroutine(Fire(enemyObj.position));

    }


    public IEnumerator Fire(Vector2 pos)
    {
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
        GameObject bullet = LeanPool.Spawn(container.ProjectilePrefab,bulletSpawnPoint.position,Quaternion.identity);
        var bulletComp = bullet.GetComponent<Bullet>();
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
        if (_lineRenderer == null) return;

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
}