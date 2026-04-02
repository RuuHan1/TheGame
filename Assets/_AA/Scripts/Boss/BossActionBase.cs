using UnityEngine;

public abstract class BossActionBase : ScriptableObject
{
    public abstract void Execute(GameObject weapon,Transform center, System.Action onComplete);
}
