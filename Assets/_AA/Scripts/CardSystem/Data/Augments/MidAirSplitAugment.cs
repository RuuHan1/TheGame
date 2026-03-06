using UnityEngine;

[CreateAssetMenu(fileName = "new Augment" , menuName = "Card System/Augments/MidAirSplitAugment")]
public class MidAirSplitAugment : AugmentSO
{
    public int splitCount = 2;
    public float splitTime = 0.2f;
    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        container.AirSplitTime = splitTime;
        container.SplitCount += splitCount; 
    }
}
