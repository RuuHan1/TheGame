using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "ScriptableObjects/BossData", order = 1)]
public class BossData : ScriptableObject
{
    [SerializeField] public float MaxHealth;
    [SerializeField] public float MovementSpeed;
    //[SerializeField] public GameObject BossWeaponPrefab;
    //[SerializeField] List<BossAction> BossActions = new();
    [SerializeField] public float MaxRange;
    [SerializeField] public float MeleeRange;
}
//[System.Serializable]
//public struct BossAction
//{
//    public BossActionType ActionType;
//    public BossActionData BossActionData;
//}
public enum BossActionType
{
    MeleeAttack,
    RangedAttack,
    MidleRangeAttack,
    SummonMinions
}
