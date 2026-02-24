using UnityEngine;

[CreateAssetMenu(menuName = "Game/Weapon/Weapon State")]
public class WeaponState : ScriptableObject
{
    private int currentSlot;

    public int CurrentSlot => currentSlot;

    public void SetSlot(int value)
    {
        currentSlot = value;
    }
}
