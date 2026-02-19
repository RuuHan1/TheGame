using UnityEngine;
[CreateAssetMenu(fileName = "StatModifier", menuName = "Card System/Modifiers/StatModifier")]
public class StatModifier : ModifierSO
{
    public float DamageMultiplier = 0f;
    public float SpeedMultiplier = 0f;
    public float CastDelay = 0f;
    public int splitCount = 0;
    public override void UpdateContainer(ProjectileContainer container, WeaponSO weapon)
    {
        container.DamageMultiplier += DamageMultiplier;
        container.SpeedMultiplier += SpeedMultiplier;
        container.CastDelay += CastDelay;
        container.SplitCount += splitCount;
        container.SetMultipliers();
    }

}
