using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyData> enemies = new();
    public List<EnemyInstance> instances = new();
    [SerializeField] private List<EnemyStatsSO> _enemyStats = new();
    [HideInInspector] private Transform _target;
    public float SpawnRate = 1f;
    private float _spawnTimer = 0f;
    //[SerializeField] private float spawnRadius = 10f;
    [SerializeField] private Transform enemyPool;
    [Header("Difficulty Scaling")]
    private float difficulty = 1f;

    //[SerializeField] float spawnScaling = 0.25f;
    [SerializeField] float hpScaling = 0.7f;
    [SerializeField] float speedScaling = 0.2f;


    [SerializeField] private float _areaRadius;
    private Transform _areaCenter;
    private int _recentKillCount = 0;
    private float _killWindowTimer = 0f;
    private const float KillWindow = 1f;
    private Vector3 _fixedOffset;
    private bool _isBossSpawned = false;
    private void OnEnable()
    {
        GameEvents.PlayerPosition += SetPlayerTarget;
        GameEvents.SpawnBoss_GameManager += OnBossSpawned;
    }

    private void OnBossSpawned()
    {
        _isBossSpawned = true;
    }

    private void OnDisable()
    {
        GameEvents.PlayerPosition -= SetPlayerTarget;
            GameEvents.SpawnBoss_GameManager -= OnBossSpawned;
    }

    private void SetPlayerTarget(Transform transform)
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle.normalized;
        _fixedOffset = (Vector3)randomPoint / 2;
        _target = transform;
        
    }
    private void Start()
    {
        _spawnTimer = SpawnRate;
        _areaCenter = transform;
    }
    void Update()
    {
        UpdateDifficulty();

        Move();

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0 && !_isBossSpawned)
        {
            SpawnEnemy();
            _spawnTimer = SpawnRate / difficulty;
        }
        if (_killWindowTimer > 0)
        {

            _killWindowTimer -= Time.deltaTime;
            if (_recentKillCount >= 7)
            {
                GameEvents.DecreaseTimeScale_EnemyManager?.Invoke();
            }
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
        if (_target == null) return;

        float dt = Time.deltaTime;
        Vector3 targetPos = _target.position;

        targetPos += _fixedOffset;
        float separationRadius = 1f;
        float separationStrength = 6f;
        float acceleration = 12f;
        float drag = 4f;
        //seperation kodu optimize edilmeli, þu an O(n^2) ama çok fazla düþman olmayacaðý için þimdilik sorun olmaz
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
            float vx = enemies[i].velocity.x;
            if (Mathf.Abs(vx) > 0.1f)
            {
                instances[i].spriteRenderer.flipX = vx < 0;
            }
            bool isMoving = enemies[i].velocity.magnitude > 0.5f;
            instances[i].trail.emitting = isMoving;
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
        GameEvents.PlayVFX_Enemy?.Invoke(VFXType.EnemyExplosion, enemies[index].position);
        GameEvents.PlaySound?.Invoke(SfxType.Sfx_enemyExplosion);
        _recentKillCount++;
        _killWindowTimer = KillWindow;

        float t = Mathf.Clamp01((float)_recentKillCount / 20f); // 20 kill = max force
        float force = Mathf.Lerp(0.1f, 3f, t);
        GameEvents.ShakeCamera_EnemyManager?.Invoke(force);
    }

    public Vector2 GetRandomPointInPlayArea()
    {
        int maxAttempts = 30;
        float minSafeDistance = 1.5f;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomPoint = (Vector2)_areaCenter.position + UnityEngine.Random.insideUnitCircle * _areaRadius;
            float distanceToTarget = Vector2.Distance(_target.transform.position, randomPoint);
            if (distanceToTarget >= minSafeDistance)
            {
                return randomPoint;
            }
        }
        return (Vector2)_areaCenter.position;
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomPointInPlayArea();
        EnemyStatsSO enemyStatsSO;
        int dice = UnityEngine.Random.Range(0, 11);
        enemyStatsSO = dice >= 10 ? _enemyStats[1] : _enemyStats[0];

        float scaledHP = enemyStatsSO.MaxHealth * (1 + difficulty * hpScaling);
        float scaledSpeed = enemyStatsSO.MoveSpeed * (1 + difficulty * speedScaling);
        float scaledDamage = enemyStatsSO.Damage * (1 + difficulty * 0.2f);

        int availableIndex = -1;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].isAlive)
            {
                availableIndex = i;
                break;
            }
        }

        GameObject newEnemy = LeanPool.Spawn(enemyStatsSO.EnemyPrefab, spawnPos, Quaternion.identity, enemyPool);
        EnemyInstance newInstance = newEnemy.GetComponent<EnemyInstance>();

        EnemyData newData = new EnemyData
        {
            position = spawnPos,
            speed = scaledSpeed,
            health = scaledHP,
            damage = scaledDamage,
            isAlive = true,
            XpWorth = enemyStatsSO.XpValue,
            velocity = Vector3.zero,
        };
        if (availableIndex != -1)
        {
            enemies[availableIndex] = newData;
            instances[availableIndex] = newInstance;
            newInstance.manger = this;
            newInstance.index = availableIndex;
        }
        else
        {
            enemies.Add(newData);
            instances.Add(newInstance);
            newInstance.manger = this;
            newInstance.index = enemies.Count - 1;
        }
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
