using UnityEngine;

//[CreateAssetMenu(fileName = "New Card", menuName = "Card System/Card")]
public abstract class CardSO : ScriptableObject
{
    public CardType CardType;
    public CardTier CardTier;
    public CardRarity CardRarity;
    public abstract void UpdateContainer(ProjectileContainer container,WeaponInstance weapon);
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
public enum CardTier
{
    Tier1, Tier2,Tier3,Tier4,Tier5
}
public enum CardRarity
{
    Common, Uncommon , Rare , Legendary 
}
