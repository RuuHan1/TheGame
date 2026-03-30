using UnityEngine;

public abstract class BossActionData : ScriptableObject
{
    [field: SerializeField] public float Range { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [SerializeField] private IBossAction _action;
    [HideInInspector] public IBossAction Action => _action;

    public abstract void DoAction();
}
