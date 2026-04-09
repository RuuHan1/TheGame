using UnityEngine;

public class GoblinBoss : Boss
{
    
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        if (_currentState == BossState.Dead) return;
        CheckCurrentState();
        switch (_currentState)
        {
            case BossState.Chase:
                MoveTowardsToPlayer();
                break;
            case BossState.MeleeAttack:
                PerformMeleeAttack();
                break;
            case BossState.RangedAttack:
                PerformRangedAttack();
                break;
        }
    }

    protected override void CheckCurrentState()
    {
        if (_isPerforming) return;
        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < _bossData.MeleeRange)
        {
            SetState(BossState.MeleeAttack);
        }
        else if (distanceToPlayer < _bossData.MaxRange)
        {
            SetState(BossState.RangedAttack);
        }
        else
        {
            SetState(BossState.Chase);
        }
    }
    
    //private void PlayAnimation(BossState state)
    //{
    //    //Play the corresponding animation based on the state
    //}
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _bossData.MeleeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _bossData.MaxRange);
    }

}


