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
    public float RechargeTime;
    public int Slots;
    public int MultiCastCount = 1;
    

    //cast caountunu arttirmak yerine ayni projectile ct den 1 tane daha koyabiliriz
    public void AddContainer(ProjectileContainer projectileContainer)
    {
            Containers.Add(projectileContainer);
    }


    public void ResetWeapon()
    {
        MultiCastCount = 1;
        triggerContainers.Clear();
    }

}
