using System.Collections.Generic;
using UnityEngine;

public class BossData : ScriptableObject
{
    [SerializeField] public float MaxHealth;
    [SerializeField] public float MovementSpeed;
    [SerializeField] public GameObject BossWeaponPrefab;
    [SerializeField] List<BossAction> BossActions = new();


}
[System.Serializable]
public struct BossAction
{
    public BossActionType ActionType;
    public BossActionData BossActionData;
}
public enum BossActionType
{
    MeleeAttack,
    RangedAttack,
    MidleRangeAttack,
    SummonMinions
}
