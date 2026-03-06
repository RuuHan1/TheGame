using System.Collections.Generic;
using UnityEngine;

//her silahýn birden fazla projectile containerý olabilir, her container farklý bir process
[CreateAssetMenu(fileName = "New Weapon", menuName = "Card System/Weapon")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private CardViewSO _defaultProjectile;
    [HideInInspector]public CardViewSO DefaultProjectile => _defaultProjectile;
    public List<ProjectileContainer> Containers = new List<ProjectileContainer>();
    public List<ProjectileContainer> triggerContainers = new List<ProjectileContainer>();
    [SerializeField] private int _baseSlots;
    [HideInInspector]public int BonusSlots;
    public float RechargeTime;
    public int TotalSlots => _baseSlots + BonusSlots;
    public int MultiCastCount = 1;
    

    //cast caountunu arttirmak yerine ayni projectile ct den 1 tane daha koyabiliriz
    public void AddContainer(ProjectileContainer projectileContainer)
    {
            Containers.Add(projectileContainer);
    }


    public void ResetWeapon()
    {
        MultiCastCount = 1;
        BonusSlots = 0;
        triggerContainers.Clear();
    }

    public void AddSlot(int amount)
    {
        BonusSlots = amount;
    }

}
