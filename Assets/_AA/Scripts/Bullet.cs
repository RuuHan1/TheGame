using Lean.Pool;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public float Damage = 1;
    public ProjectileContainer Container;
    private bool isDespawned = false;
    private Rigidbody2D rb;
    private void OnEnable()
    {
        isDespawned = false;
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (Container != null && Container.SpiralSpeed != 0)
        {
            float rotationAmount = Container.SpiralSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, 0, rotationAmount);

            rb.linearVelocity = transform.right * Container.Speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isDespawned) return;
        Vector2 hitPoint = collider.ClosestPoint(transform.position);
        Vector2 normal = (transform.position - (Vector3)hitPoint).normalized;
        collider.GetComponent<IDamagable>()?.TakeDamage(Container.Damage);
        if (Container.FragmentCount > 0)
        {
            FragmentProjectile(hitPoint, normal);

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
        //container.ResetConteiner();
        Invoke(nameof(TimedDespawn), Container.Lifetime);
        if (Container.AirSplitTime > 0 && !container.IsChildProjectile)
        {
            Invoke(nameof(ExecuteAirSplit), Container.AirSplitTime);
        }

    }

    private void SpawnPayload(ProjectileContainer payload, Vector2 position, Quaternion rotation)
    {
        if (payload.ProjectilePrefab == null) return;
        GameObject spawnedPayload = LeanPool.Spawn(payload.ProjectilePrefab, position, rotation);
        spawnedPayload.GetComponent<Bullet>().Initialize(payload.CopyContainer());
        Rigidbody2D rb = spawnedPayload.GetComponent<Rigidbody2D>();
        Vector2 direction = rotation * Vector2.right;
        rb.linearVelocity = direction * payload.Speed;

    }
    //carpisma olunca cagir
    private void FragmentProjectile(Vector2 hitPoint, Vector2 normal)
    {
        if (Container.FragmentCount <= 0) return;

        float angleStep = 360f / Container.FragmentCount;

        Vector2 baseDirection = -normal;

        float spawnOffset = Mathf.Max(Container.Radius, 0.01f);

        for (int i = 0; i < Container.FragmentCount; i++)
        {
            ProjectileContainer splitContainer = Container.CopyContainer();
            splitContainer.FragmentCount = 0;
            float currentAngle = i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * baseDirection;
            Vector2 spawnPos = hitPoint + direction * spawnOffset;
            GameObject fragment = LeanPool.Spawn(
                splitContainer.ProjectilePrefab,
                spawnPos,
                Quaternion.Euler(0, 0,
                    Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

            var rb = fragment.GetComponent<Rigidbody2D>();
            fragment.GetComponent<Bullet>().Initialize(splitContainer);
            rb.linearVelocity =
                direction * (splitContainer.Speed * 1.2f);
        }
    }
    //havada ayrilmasi icin
    private void SplitProjectile()
    {
        if (Container.SplitCount <= 0) return;

        Rigidbody2D parentRb = GetComponent<Rigidbody2D>();

        // Mevcut hareket yönü
        Vector2 baseDirection = parentRb.linearVelocity.normalized;

        // Eðer velocity çok küçükse fallback olarak transform.right kullan
        if (baseDirection.sqrMagnitude < 0.0001f)
            baseDirection = transform.right;

        float totalSpread = Container.SplitSpreadAngle; // örn 50f
        float angleStep = Container.SplitCount > 1
            ? totalSpread / (Container.SplitCount - 1)
            : 0f;

        float startAngle = -totalSpread / 2f;

        for (int i = 0; i < Container.SplitCount; i++)
        {
            ProjectileContainer splitContainer = Container.CopyContainer();
            splitContainer.SplitCount = 0;
            splitContainer.AirSplitTime = 0f;

            float currentAngle = startAngle + (angleStep * i);

            Vector2 finalDirection =
                Quaternion.Euler(0, 0, currentAngle) * baseDirection;

            Quaternion rotation =
                Quaternion.LookRotation(Vector3.forward, finalDirection);

            GameObject fragment = LeanPool.Spawn(
                splitContainer.ProjectilePrefab,
                transform.position,
                Quaternion.Euler(0, 0,
                    Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg));

            var rb = fragment.GetComponent<Rigidbody2D>();
            fragment.GetComponent<Bullet>().Initialize(splitContainer);

            rb.linearVelocity =
                finalDirection * (splitContainer.Speed * 1.2f);
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
    private void ExecuteAirSplit()
    {
        if (isDespawned) return;

        SplitProjectile();

        isDespawned = true;
        LeanPool.Despawn(this.gameObject);
    }
}
