using Lean.Pool;
using TMPro;
using UnityEngine;

public class FloatingTextManager : UIPanel
{
    [SerializeField] private TextMeshProUGUI _enemyDeathText;
    [SerializeField] private GameObject _damageTextPrefab;
    [SerializeField] private Transform _uIPrefabPool;

    private int _diedEnemies = 0;

    private void OnEnable()
    {
        GameEvents.EnemyDied += OnEnemyDied;
        GameEvents.OnEnemyDamaged += HandleEnemyDamaged;
    }
    private void OnDisable()
    {
        GameEvents.EnemyDied -= OnEnemyDied;
        GameEvents.OnEnemyDamaged -= HandleEnemyDamaged;
    }
    private void OnEnemyDied(int obj)
    {
        _diedEnemies++;
        _enemyDeathText.text = _diedEnemies.ToString();
    }
    private void HandleEnemyDamaged(Vector3 vector, float arg2, bool arg3)
    {
        Vector2 spawnPos = vector + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f), 0);
        GameObject damageText = LeanPool.Spawn(_damageTextPrefab, spawnPos, Quaternion.identity, _uIPrefabPool);

        damageText.GetComponent<DamageText>().Initialize(arg2, arg3);
    }
}
