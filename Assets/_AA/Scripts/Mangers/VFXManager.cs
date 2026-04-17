using Lean.Pool;
using UnityEngine;

[System.Serializable]
public struct VFXData
{
    public VFXType Type;
    public GameObject Prefab;
}

public class VFXManager : MonoBehaviour
{
    [SerializeField] private VFXData[] _vfxDataArray;
    private GameObject[] _prefabs;

    private void Awake()
    {
        int enumCount = (int)VFXType.COUNT;
        _prefabs = new GameObject[enumCount];

        foreach (var data in _vfxDataArray)
        {
            int index = (int)data.Type;
            _prefabs[index] = data.Prefab;
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

    private void OnPlayVFX(VFXType type, Vector2 position)
    {
        int index = (int)type;

        if (_prefabs[index] == null)
        {
            Debug.LogWarning($"Eksik VFX referansý: {type}");
            return;
        }

        LeanPool.Spawn(_prefabs[index], position, Quaternion.identity, transform);
    }
}

public enum VFXType
{
    EnemyExplosion,
    Vfx_Red,
    Vfx_Blue,
    Vfx_Yellow,
    Vfx_AddExplosion,
    LevelUp,
    COUNT
}