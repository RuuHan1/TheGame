using UnityEngine;

public interface ISceneState
{
    void Enter();
    void Tick();
    void Exit();
}
