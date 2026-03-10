using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyData> enemies = new();
    public List<EnemyInstance> instances = new();
    [SerializeField] private List<EnemyStatsSO> _enemyStats = new();
    [HideInInspector] public Transform target;
    public float SpawnRate = 1f;
    private float _spawnTimer = 0f;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private Transform enemyPool;
    [Header("Difficulty Scaling")]
    private float difficulty = 1f;

    //[SerializeField] float spawnScaling = 0.25f;
    [SerializeField] float hpScaling = 0.7f;
    [SerializeField] float speedScaling = 0.2f;

    private int _recentKillCount = 0;
    private float _killWindowTimer = 0f;
    private const float KillWindow = 1f;    
    private const int ShakeThreshold = 3;
    private const int MaxKillsForShake = 20;
    private void OnEnable()
    {
        GameEvents.PlayerPosition += SetPlayerTarget;
    }
    private void OnDisable()
    {
        GameEvents.PlayerPosition -= SetPlayerTarget;
    }

    private void SetPlayerTarget(Transform transform)
    {
        target = transform;
    }
    private void Start()
    {
        _spawnTimer = SpawnRate;
    }
    void Update()
    {
        UpdateDifficulty();

        Move();

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            SpawnEnemy();
            _spawnTimer = SpawnRate / difficulty;
        }
        if (_killWindowTimer > 0)
        {

            _killWindowTimer -= Time.deltaTime;
        }
        else
        {
            _recentKillCount = 0;

        }
    }
    void UpdateDifficulty()
    {
        difficulty = 1f + Time.timeSinceLevelLoad / 60f;
    }
    private void Move()
    {
        if (target == null) return;

        float dt = Time.deltaTime;
        Vector3 targetPos = target.position;

        float separationRadius = 1f;
        float separationStrength = 6f;
        float acceleration = 12f;
        float drag = 4f;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].isAlive)
                continue;

            EnemyData e = enemies[i];

            float currentSpeed = e.speed * (e.slowTimer > 0 ? e.slowMultiplier : 1f);
            Vector3 desiredVelocity = (targetPos - e.position).normalized * currentSpeed;

            Vector3 separation = Vector3.zero;

            for (int j = 0; j < enemies.Count; j++)
            {
                if (i == j) continue;
                if (!enemies[j].isAlive) continue;

                Vector3 diff = e.position - enemies[j].position;
                float dist = diff.magnitude;

                if (dist < separationRadius && dist > 0.001f)
                {
                    separation += diff.normalized *
                                  (separationRadius - dist);
                }
            }

            separation *= separationStrength;

            Vector3 steering = desiredVelocity + separation;
            e.velocity = Vector3.Lerp(
                e.velocity,
                steering,
                acceleration * dt
            );
            e.velocity = Vector3.Lerp(
                e.velocity,
                Vector3.zero,
                drag * dt
            );
            if (e.slowTimer > 0)
                e.slowTimer -= dt;
            e.position += e.velocity * dt;

            enemies[i] = e;
        }

        SyncViews();
    }
    void SyncViews()
    {
        for (int i = 0; i < instances.Count; i++)
        {
            if (!enemies[i].isAlive)
                continue;
            instances[i].transform.position =
                enemies[i].position;
        }
    }
    public void EnemyTookDamage(int i, float dmg)
    {
        EnemyData e = enemies[i];
        if (!e.isAlive) return;
        e.health -= dmg;

        enemies[i] = e;

        if (e.health <= 0)
        {
            GameEvents.EnemyDiedXp?.Invoke(enemies[i].position, e.XpWorth);
            KillEnemy(i);
            GameEvents.EnemyDied?.Invoke(i);

        }

    }
    private void KillEnemy(int index)
    {
        EnemyData temp = enemies[index];

        temp.isAlive = false;

        enemies[index] = temp;
        LeanPool.Despawn(instances[index].gameObject);
        _recentKillCount++;
        _killWindowTimer = KillWindow;

        float t = Mathf.Clamp01((float)_recentKillCount / 20f); // 20 kill = max force
        float force = Mathf.Lerp(0.1f, 3f, t);
        GameEvents.ShakeCamera_EnemyManager?.Invoke(force);
    }


    private void SpawnEnemy()
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = target.position + (Vector3)randomPoint;
        EnemyStatsSO enemyStatsSO; 
        int dice = UnityEngine.Random.Range(0,11);
        enemyStatsSO = dice >= 10 ? _enemyStats[1] : _enemyStats[0];
        GameObject newEnemy = LeanPool.Spawn(enemyStatsSO.EnemyPrefab, spawnPos, Quaternion.identity, enemyPool);

        float scaledHP = enemyStatsSO.MaxHealth * (1 + difficulty * hpScaling);
        float scaledSpeed = enemyStatsSO.MoveSpeed * (1 + difficulty * speedScaling);
        float scaledDamage = enemyStatsSO.Damage * (1 + difficulty * 0.2f);

        enemies.Add(new EnemyData
        {
            position = spawnPos,
            speed = scaledSpeed,
            health = scaledHP,
            damage = scaledDamage,
            isAlive = true,
            XpWorth = enemyStatsSO.XpValue,
            velocity = Vector3.zero,
        });

        instances.Add(newEnemy.GetComponent<EnemyInstance>());
        instances[^1].manger = this;
        instances[^1].index = enemies.Count - 1;
    }
    public void InvokePlayerDamaged(int index)
    {
        float damage = enemies[index].damage;

        GameEvents.PlayerDamaged?.Invoke(damage);
    }
    public void ApplyKnockback(int index, Vector3 force)
    {
        EnemyData e = enemies[index];
        e.velocity += force;
        enemies[index] = e;
    }
    public void ApplySlow(int index, float multiplier, float duration)
    {
        EnemyData e = enemies[index];
        // Daha güçlü slow varsa yoksay
        if (e.slowTimer > 0 && multiplier > e.slowMultiplier) return;
        e.slowMultiplier = multiplier;
        e.slowTimer = duration;
        enemies[index] = e;
    }
}
