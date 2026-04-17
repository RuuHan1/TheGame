using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

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

    [SerializeField] private float pickupCheckInterval = 0.3f; // Her 0.1s'de bir kontrol
    [SerializeField] private ContactFilter2D _contactFilter;
    private Collider2D[] _pickupBuffer = new Collider2D[40]; // Sabit buffer, GC yok
    private float _nextPickupCheck;
    private InputSystem_Actions _actions;
    private void OnEnable()
    {
        GameEvents.XpCollected += CollectXp;
        GameEvents.PlayerDamaged += TakeDamage;
        _actions = new InputSystem_Actions();
    }
    private void Start()
    {
        CurrentHealth = MaxHealth;

        GameEvents.PlayerHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        GameEvents.PlayerXpChanged?.Invoke(xpForNextLevel, _currentXp, _level);
    }


    private void OnDisable()
    {
        GameEvents.XpCollected -= CollectXp;
        GameEvents.PlayerDamaged -= TakeDamage;
    }
    private void Update()
    {
        if (Time.time >= _healthRegenTimer)
        {
            RegenHealth();

        }
        if (Time.time >= _nextPickupCheck)
        {
            CheckPickupRange();
            _nextPickupCheck = Time.time + pickupCheckInterval;
        }
    }

    public void CollectXp(float value)
    {
        _currentXp += value * xpGainMultiplier;
        GameEvents.PlayerXpChanged?.Invoke(xpForNextLevel, _currentXp, _level);
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
        this.GetComponent<PlayerController>().SetMoveSpeed(MoveSpeed);

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
        pickupRadius += 0.2f;
    }

    private void RegenHealth()
    {

        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += healthRegenRate;
            CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
            GameEvents.PlayerHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            GameEvents.PlayerHealthRegen_PlayerStats?.Invoke(healthRegenRate);
            _healthRegenTimer = Time.time + healthRegenInterval;
        }

    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        _healthRegenTimer = Time.time + healthRegenInterval;

        GameEvents.PlayerHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _actions.Disable();
        Time.timeScale = 0f;
        
    }
    private void CheckPickupRange()
    {

        int count = Physics2D.OverlapCircle(this.transform.position, pickupRadius, _contactFilter, _pickupBuffer);
        for (int i = 0; i < count; i++)
        {
            var collectable = _pickupBuffer[i].GetComponent<ICollectable>();
            //var interactable = _pickupBuffer[i].GetComponent<IInteractable>();
            if (collectable != null)
            {
                collectable?.Collect(this);
            }
            //if (interactable != null)
            //{
            //    interactable?.Interact();
            //}
        }
    }
}
