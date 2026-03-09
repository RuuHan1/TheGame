using UnityEngine;

[CreateAssetMenu(fileName = "New Fire Ball Card", menuName = "Card System/Projectile Cards/IceBall")]
public class IcaBall : ProjectileSO
{
    [Range(0f, 1f)]
    [SerializeField] private float _slowMultiplier = 1f;
    [Range(0f, 2f)]
    [SerializeField] private float _slowDuration = 0f;
    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        base.UpdateContainer(container, weapon);
        container.SlowMultiplier = _slowMultiplier;
        container.SlowDuration = _slowDuration;
    }
}
