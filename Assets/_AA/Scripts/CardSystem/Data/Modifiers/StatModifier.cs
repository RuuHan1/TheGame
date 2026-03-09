using UnityEngine;
[CreateAssetMenu(fileName = "StatModifier", menuName = "Card System/Modifiers/StatModifier")]
public class StatModifier : ModifierSO
{
    public float DamageMultiplier = 0f;
    public float SpeedMultiplier = 0f;
    public float CastDelay = 0f;
    public int splitCount = 0;
    //15-20 arasi olabilir
    [SerializeField] private float _knockbackForce = 0f;

    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        container.DamageMultiplier += DamageMultiplier;
        container.SpeedMultiplier += SpeedMultiplier;
        container.CastDelay += CastDelay;
        container.FragmentCount += splitCount;
        container.KnockbackForce += _knockbackForce;
        container.SetMultipliers();
    }

}
