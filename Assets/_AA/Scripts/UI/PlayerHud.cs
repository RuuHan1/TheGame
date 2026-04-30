using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : UIPanel
{
    [Header("Player UI")]
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private Image _playerXpImage;
    [SerializeField] private TMP_Text _playerLevelText;
    [SerializeField] private Transform _uIPrefabPool;
    [SerializeField] private GameObject levelUpTextPrefab;
    [SerializeField] private GameObject _worldText;
    private Transform _playerTransform;
    private void OnEnable()
    {
        GameEvents.PlayerHealthChanged += OnPlayerHealthChanged;
        GameEvents.PlayerXpChanged += OnPlayerXpChanged;
        GameEvents.PlayerLevelUp += OnSpawnPlayerLevelUpText;
        GameEvents.PlayerDamaged += OnPlayerDamaged;
        GameEvents.PlayerPosition += OnPlayerTransformAssigned;
        GameEvents.PlayerHealthRegen_PlayerStats += OnPlayerHealthRegen;

    }
    private void OnDisable()
    {
        GameEvents.PlayerHealthChanged -= OnPlayerHealthChanged;
        GameEvents.PlayerXpChanged -= OnPlayerXpChanged;
        GameEvents.PlayerLevelUp -= OnSpawnPlayerLevelUpText;
        GameEvents.PlayerDamaged -= OnPlayerDamaged;
        GameEvents.PlayerPosition -= OnPlayerTransformAssigned;
        GameEvents.PlayerHealthRegen_PlayerStats -= OnPlayerHealthRegen;
    }
    private void OnSpawnPlayerLevelUpText(Transform playerTransform,int level)
    {
        Vector2 spawnPos = playerTransform.position + Vector3.up;
        GameObject levelUpText = LeanPool.Spawn(levelUpTextPrefab, spawnPos, Quaternion.identity, _uIPrefabPool);
    }
    private void OnPlayerHealthChanged(float currentPlayerHealth, float maxHealth)
    {
        float fillAmount = currentPlayerHealth / maxHealth;
        playerHealthImage.fillAmount = fillAmount;

    }
    private void OnPlayerXpChanged(float maxXp, float currentXp, int level)
    {
        _playerLevelText.text = $"Lvl.{level}";
        float fillAmount = currentXp;
        if (currentXp > maxXp)
        {
            fillAmount = currentXp - maxXp;
        }
        fillAmount = fillAmount / maxXp;
        _playerXpImage.fillAmount = fillAmount;
    }
    private void OnPlayerHealthRegen(float obj)
    {
        Vector2 spawnPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y, 0) + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f), 0);
        GameObject healthText = LeanPool.Spawn(_worldText, spawnPos, Quaternion.identity);

        healthText.GetComponent<DamageText>().OnPlayerRegen(obj);
    }

    private void OnPlayerTransformAssigned(Transform transform)
    {
        _playerTransform = transform;
    }

    private void OnPlayerDamaged(float obj)
    {
        Vector2 spawnPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y, 0) + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f), 0);
        GameObject damageText = LeanPool.Spawn(_worldText, spawnPos, Quaternion.identity);

        damageText.GetComponent<DamageText>().Initialize(obj, Color.red);
    }
}
