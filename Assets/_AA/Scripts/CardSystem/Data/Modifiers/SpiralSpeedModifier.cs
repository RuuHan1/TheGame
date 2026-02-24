using UnityEngine;

[CreateAssetMenu(fileName = "mew Modifier",menuName = "Card System/Modifiers/SpiralSpeedModifier")]
public class SpiralSpeedModifier : ModifierSO
{
    public float rotationSpeed = 180f;
    public override void UpdateContainer(ProjectileContainer container, WeaponSO weapon)
    {
        container.SpiralSpeed = rotationSpeed;
        container.Lifetime += 2f;
    }
}
