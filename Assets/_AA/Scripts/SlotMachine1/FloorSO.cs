using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Floor Data")]
public class FloorSO : ScriptableObject
{
    [SerializeField] private List<RarityRatio> _rarityRatioList = new();
    [SerializeField] private List<TypeRatio> _typeRatioList = new();
    [SerializeField] private List<CardType> _possibleCardTypes = new();
    public IReadOnlyList<RarityRatio> RarityRatioList => _rarityRatioList;
    public IReadOnlyList<CardType> CardTypes => _possibleCardTypes;
    public IReadOnlyList<TypeRatio> TypeRatioList => _typeRatioList;
    // private int floorIndex = 0;

    public float GetTotalRatio()
    {
        float totalWeight = 0;
        foreach(var ratio in _rarityRatioList)
        {
            totalWeight += ratio.weight;
        }
        return totalWeight;
    }
    public float GetTotalTypeRatio()
    {
        float total = 0;
        foreach (var ratio in _typeRatioList)
            total += ratio.Weight;
        return total;
    }
}
[System.Serializable]
public struct RarityRatio
{
    public CardRarity Rarity;
    public float weight;
}
[System.Serializable]
public struct TypeRatio
{
    public CardType Type;
    public float Weight;
}
