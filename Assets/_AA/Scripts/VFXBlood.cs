using Lean.Pool;
using UnityEngine;

public class VFXBlood : MonoBehaviour
{
    private void OnEnable() => Invoke(nameof(Despawn),0.5f);

    private void OnDisable() => CancelInvoke();

    private void Despawn() => LeanPool.Despawn(gameObject);
}
