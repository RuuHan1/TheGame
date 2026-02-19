using UnityEngine;
using Lean.Pool;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Collections;
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


    private void OnEnable()
    {
        GameEvents.WeaponSlotChanged += OnWeaponSlotChanged;
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, range);
    //}

    private void Start()
    {
        if (currentWeaponSO != null)
        {
            ApplyCards();
        }
    }
    public IEnumerator Fire(Vector2 pos)
    {
        _isFiring = true;
        foreach (var container in currentWeaponSO.Containers)
        {
            if (container.ProjectilePrefab == null) continue;
            if (container.IsTriggered) { 
                currentWeaponSO.triggerContainers.Add(container);
                continue;
            }
            if(currentWeaponSO.triggerContainers != null)
            {
               container.OnHitPayloads = currentWeaponSO.triggerContainers;
            }
            GameObject bullet = LeanPool.Spawn(container.ProjectilePrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(container);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>(); // Example velocity, adjust as needed
            Vector2 direction = (pos - (Vector2)bulletSpawnPoint.position).normalized;
            if (rb != null)
            {
                rb.linearVelocity = direction * container.Speed;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            float delay = Math.Max(0.05f, container.CastDelay);
            yield return new WaitForSeconds(delay);
            //projectilePrefab.Remove(container.ProjectilePrefab);
            //Debug.Log($"{currentWeaponSO.Containers.Count}");
            // Debug.Log($"{projectilePrefab.Count}");
        }

        _isFiring = false;
        ApplyCards();
        _fire = Time.time + currentWeaponSO.RechargeTime;

    }

    private void SpawnBullet(ProjectileContainer container, Vector2 pos)
    {
        throw new NotImplementedException();
    }

    private void ApplyCards()
    {
        if (currentWeaponSO.Containers == null)
        {
            currentWeaponSO.Containers = new List<ProjectileContainer>();
            Debug.Log("Containers initialized in ApplyCards.");
        }

        currentWeaponSO.Containers.Clear();

        List<CardSO> pendingCards = new List<CardSO>();

        foreach (var card in cardsInSlots)
        {
            if(card.CardType == CardType.Modifier || card.CardType == CardType.Augment)
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

}