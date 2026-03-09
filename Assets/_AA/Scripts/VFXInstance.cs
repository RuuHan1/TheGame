using UnityEngine;
using Lean.Pool;
[RequireComponent(typeof(ParticleSystem))]
public class VFXInstance : MonoBehaviour
{
    private ParticleSystem _ps;

    private void Awake() => _ps = GetComponent<ParticleSystem>();

    private void OnEnable() => Invoke(nameof(Despawn), _ps.main.duration + _ps.main.startLifetime.constantMax);

    private void OnDisable() => CancelInvoke();

    private void Despawn() => LeanPool.Despawn(gameObject);
}
