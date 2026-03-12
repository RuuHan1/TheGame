using Lean.Pool;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private GameObject levelUpTextPrefab;
    [SerializeField] private TextMeshProUGUI enemyDeathText;
    [SerializeField] private Transform uIPrefabPool;
    [Header("Timer UI")]
    [SerializeField] private TMP_Text _timerText;
    private int _secondsPassed = 0;
    [Header("Player UI")]
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private Image _playerXpImage;
    [SerializeField] private TMP_Text _playerLevelText;
    private int _diedEnemies = 0;
    //[SerializeField] private GameObject weaponRechargeTime;
    private void OnEnable()
    {
        GameEvents.OnEnemyDamaged += HandleEnemyDamaged;
        GameEvents.EnemyDied += HandleEnemyDied;
        GameEvents.PlayerLevelUp += SpawnPlayerLevelUpText;
        GameEvents.PlayerHealthChanged += OnPlayerHealthChanged;
        GameEvents.PlayerXpChanged += OnPlayerXpChanged;
        GameEvents.SecondPassed += OnSecondPassed;
        
    }


    private void OnDisable()
    {
        GameEvents.OnEnemyDamaged -= HandleEnemyDamaged;
        GameEvents.EnemyDied -= HandleEnemyDied;
        GameEvents.PlayerLevelUp -= SpawnPlayerLevelUpText;
        GameEvents.PlayerHealthChanged -= OnPlayerHealthChanged;
        GameEvents.PlayerXpChanged -= OnPlayerXpChanged;
        GameEvents.SecondPassed -= OnSecondPassed;
    }
    private void OnSecondPassed()
    {
        _secondsPassed++;
        TimeSpan timeSpan = TimeSpan.FromSeconds(_secondsPassed);
        string formattedTime = timeSpan.ToString(@"mm\:ss");
        _timerText.text = formattedTime;
    }
    private void SpawnPlayerLevelUpText(Transform playerTransform)
    {
        Vector2 spawnPos = playerTransform.position + Vector3.up;
        GameObject levelUpText = LeanPool.Spawn(levelUpTextPrefab,spawnPos,Quaternion.identity,uIPrefabPool);
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
    private void OnPlayerHealthChanged( float currentPlayerHealth,float maxHealth)
    {
        float fillAmount = currentPlayerHealth/maxHealth;
        playerHealthImage.fillAmount = fillAmount;

    }
    private void OnPlayerXpChanged(float maxXp, float currentXp,int level)
    {
        _playerLevelText.text = $"Lvl.{level}";
        float fillAmount = currentXp;
        //if blogu test edilmedi
        if(currentXp > maxXp)
        {
            fillAmount = currentXp - maxXp;
        }
        fillAmount = fillAmount / maxXp;
        _playerXpImage.fillAmount = fillAmount;
    }
}
