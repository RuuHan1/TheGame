using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    private List<GameObject> tiles = new();
    [SerializeField] private GameObject tilePrefab;
    [Header("Events")]
    [SerializeField] private WeaponState weaponState;
    [SerializeField] private WeaponChangedEvent weaponChangedEvent;
    private void OnEnable()
    {
        weaponChangedEvent.OnEventRaised += OnWeaponChanged;
        OnWeaponChanged(weaponState.CurrentSlot);
    }

    private void OnDisable()
    {
        weaponChangedEvent.OnEventRaised -= OnWeaponChanged;
    }
    private void OnWeaponChanged(int value)
    {
        GenerateGrid(value);
    }
    private void GenerateGrid(int slot)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        tiles.Clear();
        for (int i = 0; i < slot; i++)
        {
            GameObject gameObject = Instantiate(tilePrefab, this.transform);
            tiles.Add(gameObject);
        }

    }
}
