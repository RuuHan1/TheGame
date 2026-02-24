using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private GameObject levelUpTextPrefab;
    [SerializeField] private TextMeshProUGUI enemyDeathText;
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private Transform uIPrefabPool;
    private int _diedEnemies = 0;
    //[SerializeField] private GameObject weaponRechargeTime;
    private void OnEnable()
    {
        GameEvents.OnEnemyDamaged += HandleEnemyDamaged;
        GameEvents.EnemyDied += HandleEnemyDied;
        GameEvents.PlayerLevelUp += SpawnPlayerLevelUpText;
        GameEvents.PlayerHealthChanged += OnPlayerHealthChanged;
    }

    private void SpawnPlayerLevelUpText(Transform playerTransform)
    {
        Vector2 spawnPos = playerTransform.position + Vector3.up;
        GameObject levelUpText = LeanPool.Spawn(levelUpTextPrefab,spawnPos,Quaternion.identity,uIPrefabPool);
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyDamaged -= HandleEnemyDamaged;
        GameEvents.EnemyDied -= HandleEnemyDied;
        GameEvents.PlayerLevelUp -= SpawnPlayerLevelUpText;
        GameEvents.PlayerHealthChanged -= OnPlayerHealthChanged;
    }
    private void HandleEnemyDied(int obj)
    {
        _diedEnemies++;
        enemyDeathText.text = _diedEnemies.ToString();  
    }
    private void HandleEnemyDamaged(Vector3 vector, float arg2, bool arg3)
    {
        Vector2 spawnPos = vector + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f), 0);
        GameObject damageText = LeanPool.Spawn(damageTextPrefab, spawnPos, Quaternion.identity,uIPrefabPool);

        damageText.GetComponent<DamageText>().Initialize(arg2, arg3);
    }
    private void OnPlayerHealthChanged(float damage , float currentPlayerHealth)
    {
        float fillAmount = currentPlayerHealth - damage;

        fillAmount = fillAmount / 100f;
        //fillAmount = Mathf.Max(fillAmount,0);
        playerHealthImage.fillAmount = fillAmount;

    }
}
