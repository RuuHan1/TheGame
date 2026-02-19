using Lean.Pool;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public float Damage = 1;
    public ProjectileContainer Container;
    private bool isDespawned = false;
    private void OnEnable()
    {
        isDespawned = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isDespawned) return;
        collider.GetComponent<IDamagable>()?.TakeDamage(Container.Damage);
        if (Container.SplitCount > 0 ) 
        {
            SplitProjectile();

        }
        if (Container.OnHitPayloads != null && Container.OnHitPayloads.Count > 0)
        {
            foreach (var payload in Container.OnHitPayloads)
            {
                
                SpawnPayload(payload, transform.position, transform.rotation);
            }
        }
        //Container.ResetConteiner();
        isDespawned = true;
        LeanPool.Despawn(this.gameObject);
    }
    public void Initialize(ProjectileContainer container)
    {
        Container = container;
        Invoke(nameof(TimedDespawn), Container.Lifetime);

    }

    private void SpawnPayload(ProjectileContainer payload, Vector2 position, Quaternion rotation)
    {
        if (payload.ProjectilePrefab == null) return;
        GameObject spawnedPayload = LeanPool.Spawn(payload.ProjectilePrefab, position, rotation);
        spawnedPayload.GetComponent<Bullet>().Initialize(payload);
        Rigidbody2D rb = spawnedPayload.GetComponent<Rigidbody2D>();
        Vector2 direction = rotation * Vector2.right;
        rb.linearVelocity = direction * payload.Speed;
        
    }
    private void SplitProjectile()
    {
        float angleStep = 360f / Container.SplitCount;
        for (int i = 0; i < Container.SplitCount; i++)
        {
            ProjectileContainer splitContainer = Container.CopyContainer();
            
            splitContainer.SplitCount = 0;
            
            float currentAngle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + currentAngle);
            GameObject fragment = LeanPool.Spawn(splitContainer.ProjectilePrefab, transform.position, rotation);
            var rb = fragment.GetComponent<Rigidbody2D>();
            fragment.GetComponent<Bullet>().Initialize(splitContainer);
            rb.linearVelocity = (rotation * Vector2.right) * (splitContainer.Speed * 1.2f);
            
        }
    }
    private void TimedDespawn()
    {
        if (!isDespawned)
        {
            isDespawned = true;
            LeanPool.Despawn(gameObject);
        }
    }
}
