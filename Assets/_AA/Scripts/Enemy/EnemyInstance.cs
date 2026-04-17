using UnityEngine;

public class EnemyInstance : MonoBehaviour, IDamagable , IKnockbackable,ISlowable
{
    [HideInInspector]public int index;
    
    [HideInInspector] public EnemyManager manger;
    [SerializeField] private float damageInterval = 1f; 
    private float nextDamageTime = 0f;
    [HideInInspector] public TrailRenderer trail;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ApplyKnockback(Vector3 force)
    {
        manger.ApplyKnockback(index, force);
    }

    public void ApplySlow(float multiplier, float duration)
    {
        manger.ApplySlow(index, multiplier, duration);
    }

    public void TakeDamage(float damage)
    {
        GameEvents.OnEnemyDamaged?.Invoke(transform.position, damage, false);
        manger.EnemyTookDamage(index, damage);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time < nextDamageTime) return;

        var player = collision.GetComponent<PlayerStats>();
        if (player != null)
        {
            manger.InvokePlayerDamaged(index);

            nextDamageTime = Time.time + damageInterval;
        }
    }
    private void OnDisable()
    {
        if (trail != null) trail.Clear();
    }
}
