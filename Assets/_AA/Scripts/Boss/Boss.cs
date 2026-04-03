using UnityEngine;

public abstract class Boss : MonoBehaviour,IDamagable
{
    [SerializeField] protected BossData _bossData;
    [SerializeField] protected BossHealth _bossHealth;
    protected BossState _currentState = BossState.Chase;
    protected Transform _playerTransform;
    protected bool _isPerforming;
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
        throw new System.NotImplementedException();
    }
}
public enum BossState
{
    Chase,
    MeleeAttack,
    RangedAttack,
}