using System;
using UnityEngine;

public abstract class EventChannelSO<T> : ScriptableObject
{
    private event Action<T> _OnEvent;
    public void RaiseEvent(T value)
    {
        _OnEvent?.Invoke(value);
    }

    public void Register(Action<T> listener)
    {
        _OnEvent += listener;
    }

    public void Unregister(Action<T> listener) 
    {
        _OnEvent -= listener;
    }

    public void Clear()
    {
        _OnEvent = null;
    }


}
