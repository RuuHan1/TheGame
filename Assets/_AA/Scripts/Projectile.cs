using Lean.Pool;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //public float Damage = 1;
    private ProjectileContainer _container;
    private bool isDespawned = false;
    private Rigidbody2D rb;
    private bool _hasExploded = false;
    private void OnEnable()
    {
        isDespawned = false;
        _hasExploded = false;
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
        if (_container != null && _container.RotationSpeed != 0)
        {
            float rotationAmount = _container.RotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, 0, rotationAmount);

            rb.linearVelocity = transform.right * _container.Speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isDespawned || _hasExploded) return;
        Vector2 hitPoint = collider.ClosestPoint(transform.position);
        Vector2 normal = (transform.position - (Vector3)hitPoint).normalized;

        collider.GetComponent<IDamagable>()?.TakeDamage(_container.Damage);
        GameEvents.PlayVFX_Projectile?.Invoke(_container.VFXKey, hitPoint);
        Vector3 knockbackDir = (collider.transform.position - transform.position).normalized;
        if (_container.KnockbackForce > 0)
        {
            collider.GetComponent<IKnockbackable>()?.ApplyKnockback(knockbackDir * _container.KnockbackForce);
        }
        if (_container.SlowDuration > 0f)
        {
            collider.GetComponent<ISlowable>()?.ApplySlow(_container.SlowMultiplier, _container.SlowDuration);

        }
        if (_container.Radius > 0)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(hitPoint, _container.Radius);
            foreach (var hitCollider in hitColliders)
            {
                //kendime damage vermek icin bunu kaldirabilirim
                if (hitCollider == collider || hitCollider.GetComponent<PlayerStats>()) continue;
                hitCollider.GetComponent<IDamagable>()?.TakeDamage(_container.Damage);
                if (_container.KnockbackForce > 0)
                {
                    collider.GetComponent<IKnockbackable>()?.ApplyKnockback(knockbackDir * _container.KnockbackForce);
                }
            }
            _hasExploded = true;
        }


        if (_container.FragmentCount > 0)
        {
            FragmentProjectile(hitPoint, normal);

        }
        if (_container.OnHitPayloads != null && _container.OnHitPayloads.Count > 0)
        {
            foreach (var payload in _container.OnHitPayloads)
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
        _container = container;
        //container.ResetConteiner();
        Invoke(nameof(TimedDespawn), _container.Lifetime);
        if (_container.AirSplitTime > 0 && !container.IsChildProjectile)
        {
            Invoke(nameof(ExecuteAirSplit), _container.AirSplitTime);
        }

    }

    private void SpawnPayload(ProjectileContainer payload, Vector2 position, Quaternion rotation)
    {
        if (payload.ProjectilePrefab == null) return;
        GameObject spawnedPayload = LeanPool.Spawn(payload.ProjectilePrefab, position, rotation);
        spawnedPayload.GetComponent<Projectile>().Initialize(payload.CopyContainer());
        Rigidbody2D rb = spawnedPayload.GetComponent<Rigidbody2D>();
        Vector2 direction = rotation * Vector2.right;
        rb.linearVelocity = direction * payload.Speed;

    }
    //carpisma olunca cagir
    private void FragmentProjectile(Vector2 hitPoint, Vector2 normal)
    {
        if (_container.FragmentCount <= 0) return;

        float angleStep = 360f / _container.FragmentCount;

        Vector2 baseDirection = -normal;

        float spawnOffset = Mathf.Max(_container.Radius, 0.01f);

        for (int i = 0; i < _container.FragmentCount; i++)
        {
            ProjectileContainer splitContainer = _container.CopyContainer();
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
            fragment.GetComponent<Projectile>().Initialize(splitContainer);
            rb.linearVelocity =
                direction * (splitContainer.Speed * 1.2f);
        }
    }
    //havada ayrilmasi icin
    private void SplitProjectile()
    {
        if (_container.SplitCount <= 0) return;

        Rigidbody2D parentRb = GetComponent<Rigidbody2D>();

        // Mevcut hareket yönü
        Vector2 baseDirection = parentRb.linearVelocity.normalized;

        // Eðer velocity çok küçükse fallback olarak transform.right kullan
        if (baseDirection.sqrMagnitude < 0.0001f)
            baseDirection = transform.right;

        float totalSpread = _container.SplitSpreadAngle; // örn 50f
        float angleStep = _container.SplitCount > 1
            ? totalSpread / (_container.SplitCount - 1)
            : 0f;

        float startAngle = -totalSpread / 2f;

        for (int i = 0; i < _container.SplitCount; i++)
        {
            ProjectileContainer splitContainer = _container.CopyContainer();
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
            fragment.GetComponent<Projectile>().Initialize(splitContainer);

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
