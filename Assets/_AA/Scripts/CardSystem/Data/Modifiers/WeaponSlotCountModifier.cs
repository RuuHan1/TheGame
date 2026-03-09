using UnityEngine;
[CreateAssetMenu(menuName = "Card System/Modifiers/Weapon Slot Count Modifier")]
public class WeaponSlotCountModifier : ModifierSO
{
    [SerializeField] private int slotCountIncrease = 2;

    public override void UpdateContainer(ProjectileContainer container, WeaponInstance weapon)
    {
        weapon.AddSlot(slotCountIncrease);
    }
}
