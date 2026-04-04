using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BossData", menuName = "ScriptableObjects/BossData", order = 1)]
public class BossData : ScriptableObject
{
    [SerializeField] public float MaxHealth;
    [SerializeField] public float Damage;
    [SerializeField] public float MovementSpeed;
    [SerializeField] public float MaxRange;
    [SerializeField] public float MeleeRange;
    [SerializeField] public GameObject BossWeaponPrefab;
    [SerializeField] public List<BossAction> BossActions = new();
    public BossActionBase GetAction(BossState state)
    {
        List<BossActionBase> actions = new();
        for (int i = 0; i < BossActions.Count; i++)
        {
            if (BossActions[i].State == state)
            {
                actions.Add(BossActions[i].BossAct);
            }
        }
        if (actions.Count > 0)
        {
            return actions[Random.Range(0, actions.Count)];
        }
        return null;
    }
    public BossActionBase GetRandomAction(BossState state)
    {
        List<BossActionBase> actions = new();
        for (int i = 0; i < BossActions.Count; i++)
        {
            if (BossActions[i].State == state)
            {
                actions.Add(BossActions[i].BossAct);
               
            }
        }
        if (actions.Count > 0)
        {
            return actions[Random.Range(0, actions.Count)];
        }
        return null;
    }
}


[System.Serializable]
public struct BossAction
{
    public BossState State;
    public BossActionBase BossAct;
}

//public enum BossActionType
//{
//    MeleeAttack,
//    RangedAttack,
//    MidleRangeAttack,
//    SummonMinions
//}
