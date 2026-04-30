using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardLibrarySO _cardLibrary;
    [SerializeField] private RunDataSO _runCardDataSO;
    [SerializeField] private List<CardSO> _allCardDatas;
    [HideInInspector] public IReadOnlyList<CardSO> AllCards => _allCardDatas;
    //sadece izlemek icin ,  daha sonra private yapilabilir
    public List<CardViewSO> Hand = new();
    public List<CardViewSO> WeaponSlot = new();
    private void OnEnable()
    {
        GameEvents.HandChanged += UpdateHand;
        GameEvents.WeaponSlotChanged += UpdateWeapon;
        GameEvents.CardAwarded += AddCardToHand;
    }

    private void OnDisable()
    {
        GameEvents.HandChanged -= UpdateHand;
        GameEvents.WeaponSlotChanged -= UpdateWeapon;
        GameEvents.CardAwarded -= AddCardToHand;
    }

    //private void Start()
    //{
    //    AddCardToHand(_cardLibrary.CardViews[0]);
    //    GameEvents.HandChanged?.Invoke(Hand);
    //}

    private void UpdateHand(List<CardViewSO> list)
    {
        Hand = new List<CardViewSO>(list);
        _runCardDataSO.HandCards.Clear();
        _runCardDataSO.HandCards.AddRange(list);
    }

    public void AddCardToHand(CardViewSO card)
    {
        Hand.Add(card);
        GameEvents.HandChanged?.Invoke(Hand);
    }
    private void UpdateWeapon(List<CardViewSO> list)
    {
        WeaponSlot = new List<CardViewSO>(list);
        _runCardDataSO.WeaponCards.Clear();
        _runCardDataSO.WeaponCards.AddRange(list);
    }


}
