using UnityEngine;

[CreateAssetMenu(fileName = "mew Modifier",menuName = "Card System/Modifiers/SpiralSpeedModifier")]
public class SpiralSpeedModifier : ModifierSO
{
    public float RotationSpeed = 180f;
    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        container.RotationSpeed = RotationSpeed;
        container.Lifetime += 2f;
    }
}
