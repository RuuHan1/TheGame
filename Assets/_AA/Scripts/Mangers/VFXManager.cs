using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private Dictionary<string, GameObject> _vfxPrefabs = new ();
    [SerializeField] private List<VFXPrefab> _vfxPrefabsList;
    private void Awake()
    {
        foreach (var vfx in _vfxPrefabsList)
        {
            if (!_vfxPrefabs.ContainsKey(vfx.Key))
            {
                _vfxPrefabs.Add(vfx.Key, vfx.Prefab);
            }
            else
            {
                Debug.LogWarning($"Duplicate VFX key detected: {vfx.Key}. Skipping.");
            }
        }
    }
    private void OnEnable()
    {
        GameEvents.PlayVFX_Projectile += OnPlayVFX;
        GameEvents.PlayVFX_Enemy += OnPlayVFX;
    }
    private void OnDisable()
    {
        GameEvents.PlayVFX_Projectile -= OnPlayVFX;
        GameEvents.PlayVFX_Enemy -= OnPlayVFX;
    }
    private void OnPlayVFX(string key,Vector2 position)
    {
        foreach (var vfx in _vfxPrefabs)
        {
            if (vfx.Key == key)
            {
                LeanPool.Spawn(vfx.Value, position, Quaternion.identity,this.transform);
                break;
            }
        }
    }
}
[System.Serializable]
public struct VFXPrefab
{
    public string Key;
    public GameObject Prefab;
}