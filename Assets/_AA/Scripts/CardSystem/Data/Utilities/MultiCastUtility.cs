using UnityEngine;

[CreateAssetMenu(fileName = "New Utility", menuName ="Card System/Utility/Multicast")]
public class MultiCastUtility : UtilitySO
{
    [SerializeField] private int multiCast = 0;
    public override void UpdateContainer(ProjectileContainer container, WeaponSO weapon)
    {
        weapon.MultiCastCount += multiCast;
    }
}
