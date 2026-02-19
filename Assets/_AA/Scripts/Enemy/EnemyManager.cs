using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyData> enemies = new();
    public List<EnemyInstance> instances = new();
    [SerializeField] private EnemyStatsSO enemyStats;
    [HideInInspector] public Transform target;
    public float SpawnRate = 1f;
    private float _spawnTimer = 0f;
    [SerializeField] private float spawnRadius = 10f;
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
        Move();


        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            SpawnEnemy();
            _spawnTimer = SpawnRate;
        }
    }

    private void Move()
    {
        float dt = Time.deltaTime;
        Vector3 targetPos = target.position;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].isAlive)
                continue;

            EnemyData e = enemies[i];

            Vector3 dir = (targetPos - e.position).normalized;
            e.position += dir * e.speed * dt;

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
    }


    private void SpawnEnemy()
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = target.position + (Vector3)randomPoint;
        GameObject newEnemy = LeanPool.Spawn(enemyStats.EnemyPrefab, spawnPos, Quaternion.identity);
        enemies.Add(new EnemyData
        {
            position = spawnPos,
            speed = enemyStats.MoveSpeed,
            health = enemyStats.MaxHealth,
            damage = enemyStats.Damage,
            isAlive = true,
            XpWorth = enemyStats.XpValue
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
}
