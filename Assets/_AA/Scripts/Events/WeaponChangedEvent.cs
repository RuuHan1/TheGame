using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/WeaponChangedEvent")]
public class WeaponChangedEvent : ScriptableObject
{
    public Action<int> OnEventRaised;

    public void RaiseEvent(int value)
    {
        OnEventRaised?.Invoke(value);
    }

}
