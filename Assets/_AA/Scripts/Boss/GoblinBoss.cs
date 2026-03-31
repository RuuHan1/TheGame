using UnityEngine;

public class GoblinBoss : Boss
{
    [SerializeField] private BossData bossData;
    private BossState _currentState;
    private Transform _playerTransform;
    private bool _isPerforming;
    private void Update()
    {
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

    private void CheckCurrentState()
    {
        if (_isPerforming) return;
        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < bossData.MeleeRange)
        {
            SetState(BossState.MeleeAttack);
        }
        else if (distanceToPlayer < bossData.MaxRange)
        {
            SetState(BossState.RangedAttack);
        }
        else
        {
            SetState(BossState.Chase);
        }
    }
    private void MoveTowardsToPlayer()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        transform.position += (Vector3)direction * bossData.MovementSpeed * Time.deltaTime;
        if (Vector2.Distance(transform.position, _playerTransform.position) < bossData.MaxRange)
        {
            Debug.Log("Player is within range, switching to attack state");
            return;
        }
    }
    private void PerformMeleeAttack()
    {
        if (_isPerforming) return;
        _isPerforming = true;
        // Implement melee attack logic
        Debug.Log("Performing Melee Attack");
        //_isDoSomething = true;
         _isPerforming = false;
    }
    private void PerformRangedAttack()
    {
        if (_isPerforming) return;
        _isPerforming = true;

        // Implement ranged attack logic
        Debug.Log("Performing Ranged Attack");
        _isPerforming = false;
        // After attack is done, set _isDoSomething to false
    }
    private void SetState(BossState state)
    {
        if (_currentState == state) return;
        _currentState = state;
        _isPerforming = false;

    }
    private void PlayAnimation(BossState state)
    {
        //Play the corresponding animation based on the state
    }



    public override void SetTarget(Transform target)
    {
        _playerTransform = target;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bossData.MeleeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, bossData.MaxRange);
    }

}
public enum BossState
{
    Chase,
    MeleeAttack,
    RangedAttack,
}

