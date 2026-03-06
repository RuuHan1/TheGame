using System.Collections.Generic;
using UnityEngine;

//her silahın birden fazla projectile containerı olabilir, her container farklı bir process
[CreateAssetMenu(fileName = "New Weapon", menuName = "Card System/Weapon")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private CardViewSO _defaultProjectile;
    [HideInInspector]public CardViewSO DefaultProjectile => _defaultProjectile;
    [SerializeField] private int _baseSlots;
    [HideInInspector] public int BaseSlots => _baseSlots;
    //public List<ProjectileContainer> Containers = new List<ProjectileContainer>();
    //public List<ProjectileContainer> triggerContainers = new List<ProjectileContainer>();
    public float RechargeTime;
    //public int MultiCastCount = 1;
    

    //cast caountunu arttirmak yerine ayni projectile ct den 1 tane daha koyabiliriz
   

}
