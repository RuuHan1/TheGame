using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamagable
{
    public float MaxHealth = 100f;
    public float CurrentHealth { get; private set; }
    public float healthRegenRate = 0.2f;
    public float MoveSpeed = 5f;
    public float pickupRadius = 2f;
    public float xpGainMultiplier = 1f;
    public float xpForNextLevel = 100f;
    private float _currentXp = 0f;
    [SerializeField] private float xpGapPerLevel = 70f;
    [SerializeField] private int _level = 1;
    [SerializeField] private float healthRegenInterval = 1f;
    private float _healthRegenTimer = 0f;

    private void OnEnable()
    {
        GameEvents.XpCollected += CollectXp;
        GameEvents.PlayerDamaged += TakeDamage;
    }
    private void Start()
    {
        CurrentHealth = MaxHealth;
        GameEvents.PlayerHealthChanged?.Invoke(0, CurrentHealth);
        GameEvents.PlayerXpChanged?.Invoke(xpForNextLevel, _currentXp,_level);
    }


    private void OnDisable()
    {
        GameEvents.XpCollected -= CollectXp;
        GameEvents.PlayerDamaged -= TakeDamage;
    }
    private void Update()
    {
        if (Time.time < _healthRegenTimer) return;
        RegenHealth();
    }

    public void CollectXp(float value)
    {
        _currentXp += value * xpGainMultiplier;
        GameEvents.PlayerXpChanged?.Invoke(xpForNextLevel,_currentXp,_level);
        while (_currentXp >= xpForNextLevel)
        {
            _currentXp -= xpForNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _level++;
        xpForNextLevel += xpGapPerLevel * _level;
        IncreasePlayerStats();
        GameEvents.PlayerLevelUp?.Invoke(this.transform);
        //efekt icin event tetikle
        //Debug.Log("Level Up! Current Level: " + Level);
        //hangi ozelligini yukseltmek istiyorsan ona gore event tetikle
    }

    //suanlik butun ozellikleri yukselttigimiz bir metot tetikliyorum

    private void IncreasePlayerStats()
    {
        MaxHealth += 10f;
        MoveSpeed += 0.5f;
        healthRegenRate += 0.2f;
    }

    private void RegenHealth()
    {

        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += healthRegenRate;
            CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
            GameEvents.PlayerHealthChanged?.Invoke(0, CurrentHealth);
            _healthRegenTimer = Time.time + healthRegenInterval;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collectable = collision.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.Collect(this);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        _healthRegenTimer = Time.time + healthRegenInterval; // regen gecikmesi

        GameEvents.PlayerHealthChanged?.Invoke(damage, CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
       Time.timeScale = 0f;
    }
}
