using UnityEngine;

//[CreateAssetMenu(fileName = "New Card", menuName = "Card System/Card")]
public abstract class CardSO : ScriptableObject
{
    public CardType CardType;


    public abstract void UpdateContainer(ProjectileContainer container,WeaponSO weapon);
}
public enum CardType
{
    //mermiler
    Projectile,
    //carpanlar
    Modifier,
    //carpinca tetikleme yapar
    Augment,
    Utility

}
