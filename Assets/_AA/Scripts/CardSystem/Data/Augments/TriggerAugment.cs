using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Trigger Augment", menuName = "Card System/Augments/Trigger Augment")]
public class TriggerAugment : AugmentSO
{
    public override void UpdateContainer(ProjectileContainer container, WeaponSO weapon)
    {
        container.IsTriggered = true;
    }

}
