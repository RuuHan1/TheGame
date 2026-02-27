using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotMachine : MonoBehaviour,IInteractable
{
    private FloorSO _floorData;
    private CardManager _cardManager;

    public void Initialize(CardManager manager,FloorSO floor)
    {
        this._cardManager = manager;
        _floorData = floor;
    }


    private CardRarity GetCardRarity()
    {
        FloorSO floor = _floorData;
        int dice = Random.Range(0, (int)floor.GetTotalRatio() + 1);
        if (dice <= floor.RarityRatioList[0].weight)
        {
            return CardRarity.Common;
        }
        else if (dice <= floor.RarityRatioList[1].weight) 
        {
            return CardRarity.Uncommon;
        }
        else if(dice <= floor.RarityRatioList[2].weight)
        {
            return CardRarity.Rare;
        }
        else if(dice <= floor.RarityRatioList[3].weight)
        {
            return CardRarity.Legendary;
        }
        return CardRarity.Common;

    }

    private CardType GetCardType()
    {
        var cardTypes = _floorData.CardTypes;

        if (cardTypes == null || cardTypes.Count == 0)
        {
            Debug.LogError("CardTypes listesi boþ veya null.");
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, cardTypes.Count);

        return cardTypes[randomIndex];
    }



    public CardViewSO SpinWheel(IReadOnlyList<CardViewSO> list)
    {
        var validCardViews = list
         .Where(x => x.CardData.CardType == GetCardType() && x.CardData.CardRarity == GetCardRarity())
         .ToList();

        if (validCardViews.Count > 0)
        {
            CardViewSO selectedCardView = validCardViews[UnityEngine.Random.Range(0, validCardViews.Count)];

            Debug.Log($"Rarity : {selectedCardView.CardData.CardRarity} / Type : {selectedCardView.CardData.CardType}");
            return selectedCardView;
        }

        return null;
    }

    public void Interact()
    {
        CardViewSO card = SpinWheel(_cardManager.AllCardViews);
        _cardManager.AddCardToHand(card);
    }
}
