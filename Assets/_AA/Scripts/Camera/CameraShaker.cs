using UnityEngine;
using Unity.Cinemachine;
using System;
public class CameraShaker : MonoBehaviour
{

    private CinemachineImpulseSource _impulsSource;

    private void Start()
    {
        _impulsSource = GetComponent<CinemachineImpulseSource>();
        if (_impulsSource == null)
        {
            Debug.LogError("CinemachineImpulseSource component not found on CameraShaker GameObject.");
        }
    }
    private void OnEnable()
    {
        GameEvents.ShakeCamera_EnemyManager += ShakeCamera;
    }
    private void OnDisable()
    {
        GameEvents.ShakeCamera_EnemyManager -= ShakeCamera;
    }
    private void ShakeCamera(float obj)
    {
        _impulsSource.GenerateImpulse(obj);
    }
}
