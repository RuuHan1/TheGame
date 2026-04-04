using UnityEngine;

public abstract class Boss : MonoBehaviour,IDamagable
{
    [SerializeField] public BossData _bossData;
    private BossHealth _bossHealth;
    protected BossState _currentState = BossState.Chase;
    protected Transform _playerTransform;
    protected bool _isPerforming;

    protected virtual void OnEnable()
    {
        GameEvents.EnemySwordHit += OnEnemySwordHit;
    }
    protected virtual void OnDisable()
    {
        GameEvents.EnemySwordHit -= OnEnemySwordHit;
    }

    protected virtual void Start()
    {
        _bossHealth = GetComponent<BossHealth>();
        _bossHealth.Initialize(_bossData.MaxHealth);
    }
    protected abstract void CheckCurrentState();
    public virtual void SetTarget(Transform target) 
    {
        _playerTransform = target;
    }
    protected virtual void PerformMeleeAttack()
    {
        if (_isPerforming) return;
        _isPerforming = true;
        _bossData.GetAction(_currentState)?.Execute(_bossData.BossWeaponPrefab, transform, onComplete: () => _isPerforming = false);
    }
    protected virtual void PerformRangedAttack()
    {
        if (_isPerforming) return;
        _isPerforming = true;
        _bossData.GetAction(_currentState)?.Execute(_bossData.BossWeaponPrefab, transform, onComplete: () => _isPerforming = false);
    }
    protected virtual void SetState(BossState state)
    {
        if (_currentState == state) return;
        _currentState = state;
        _isPerforming = false;
    }
    protected virtual void MoveTowardsToPlayer()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        transform.position += (Vector3)direction * _bossData.MovementSpeed * Time.deltaTime;
        if (Vector2.Distance(transform.position, _playerTransform.position) < _bossData.MaxRange)
        {
            return;
        }
    }

    public void TakeDamage(float damage)
    {
        _bossHealth.TakeDamage(damage);
        GameEvents.OnEnemyDamaged?.Invoke(transform.position, damage, true);
    }
    protected void OnEnemySwordHit()
    {
        GameEvents.PlayerDamaged?.Invoke(_bossData.Damage);
    }
}
public enum BossState
{
    Chase,
    MeleeAttack,
    RangedAttack,
}