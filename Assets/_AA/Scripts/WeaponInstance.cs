using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance
{
    [field :SerializeField] public WeaponSO weaponData { get; private set; }
    public int currentSlots;
    public int currentMultiCastCount;
    public List<ProjectileContainer> Containers = new List<ProjectileContainer>();
    public List<ProjectileContainer> triggerContainers = new List<ProjectileContainer>();
    [HideInInspector]public int BonusSlots;
    public int BaseSlots;
    public int TotalSlots => BaseSlots + BonusSlots;
    public float RechargeTime;
    public int MultiCastCount = 1;
    public CardViewSO DefaultProjectile;
    public WeaponInstance(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
        DefaultProjectile = weaponData.DefaultProjectile;
        BaseSlots = weaponData.BaseSlots;
        RechargeTime = weaponData.RechargeTime;
    }
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
