using UnityEngine;
[CreateAssetMenu(fileName = "New Cast Count Augment", menuName = "Card System/Augments/Cast Count Augment")]
public class CastCountAugment : AugmentSO
{
    [Tooltip("projectile 2 kere ateslenir")]
    [SerializeField] private int additionalCastCount = 2;
    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        for (int i = 0; i < additionalCastCount; i++)
        {
            ProjectileContainer copy = container.CopyContainer();
            weapon.AddContainer(copy);
        }
    }
}
