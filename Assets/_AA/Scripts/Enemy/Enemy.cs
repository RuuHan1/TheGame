using UnityEngine;

public abstract class Enemy : MonoBehaviour,IDamagable
{

    [SerializeField] protected EnemyStatsSO stats;

    protected Transform target;
    protected bool isDead = false;
    
    
    public void TakeDamage(float damage)
    {
        GameEvents.OnEnemyDamaged?.Invoke(transform.position, damage, false);
    }
    public virtual void Initialize(Transform playerTarget)
    {
        target = playerTarget;
        isDead = false;
    }
    private void MoveTowardsPlayer()
    {

    }
}
