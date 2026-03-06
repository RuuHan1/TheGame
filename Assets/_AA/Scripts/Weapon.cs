using UnityEngine;
using Lean.Pool;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Collections;
using Unity.VisualScripting;
public class Weapon : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float range = 5f;
    [SerializeField] private Transform bulletSpawnPoint;

    private float _fire = 0f;
    //suanlik deaktif
    [SerializeField] private WeaponSO currentWeaponSO;
    [SerializeField] private List<CardSO> cardsInSlots = new List<CardSO>();
    private bool _isFiring = false;
    [Header("Events")]
    [SerializeField] private WeaponChangedEvent weaponChangedEvent;
    [SerializeField] private WeaponState weaponState;

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

    private void Start()
    {
        if (currentWeaponSO != null)
        {
            GameEvents.CardAwarded?.Invoke(currentWeaponSO.DefaultProjectile);
            ApplyCards();
            SetWeapon(currentWeaponSO.TotalSlots);
        }
    }

    private void OnWeaponSlotChanged(List<CardViewSO> list)
    {
        cardsInSlots.Clear();
        var temp = new List<CardViewSO>(list);
        foreach (CardViewSO card in temp) 
        {
            cardsInSlots.Add(card.CardData);
        }
        
        
    }

    private void Update()
    {
        if (Time.time < _fire) return;
        if (currentWeaponSO.Containers == null) return;
        CheckDistance();
    }
    private void CheckDistance()
    {
        if (_isFiring) return;

        var enemy = Physics2D.OverlapCircle(transform.position, range, enemyLayer);
        if (enemy == null) return;
        Transform enemyObj = enemy.GetComponent<Transform>();
        StartCoroutine(Fire(enemyObj.position));

    }


    public IEnumerator Fire(Vector2 pos)
    {
        _isFiring = true;

        var containers = currentWeaponSO.Containers;
        var triggers = currentWeaponSO.triggerContainers;
        float multiCastSpreadAngle = 10f;
        for (int i = 0; i < containers.Count;)
        {
            int batchCount = Mathf.Max(1, currentWeaponSO.MultiCastCount);
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
        currentWeaponSO.ResetWeapon();
        _isFiring = false;
        ApplyCards();
        _fire = Time.time + currentWeaponSO.RechargeTime;
    }

    private void SpawnBullet(ProjectileContainer container, Vector2 targetPos, float angleOffset)
    {
        GameObject bullet = LeanPool.Spawn(
            container.ProjectilePrefab,
            bulletSpawnPoint.position,
            Quaternion.identity);

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
        if (currentWeaponSO.Containers == null)
        {
            currentWeaponSO.Containers = new List<ProjectileContainer>();
        }

        currentWeaponSO.Containers.Clear();

        List<CardSO> pendingCards = new List<CardSO>();

        foreach (var card in cardsInSlots)
        {
            if(card.CardType == CardType.Modifier || card.CardType == CardType.Augment || card.CardType == CardType.Utility)
            {
                pendingCards.Add(card);
            }
            else if (card.CardType == CardType.Projectile)
            {
                ProjectileContainer newContainer = new ProjectileContainer();
                card.UpdateContainer(newContainer, currentWeaponSO);
                foreach (var pendingCard in pendingCards)
                {
                    pendingCard.UpdateContainer(newContainer, currentWeaponSO);
                }
                pendingCards.Clear();
                currentWeaponSO.Containers.Add(newContainer);
            }

        }

    }
    public void OnWeaponSlotAmountChanged(int count)
    {
        currentWeaponSO.BonusSlots += count;
        SetWeapon(currentWeaponSO.TotalSlots);
    }
    public void SetWeapon(int slot)
    {
       
        weaponState.SetSlot(slot);
        weaponChangedEvent.RaiseEvent(slot);
    }

}