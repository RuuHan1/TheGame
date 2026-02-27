using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<CardViewSO> _allCardViews = new();
    [SerializeField] private List<CardSO> _allCardDatas;
    [HideInInspector] public IReadOnlyList<CardSO> AllCards => _allCardDatas;
    [HideInInspector] public IReadOnlyList<CardViewSO> AllCardViews => _allCardViews;
    public List<CardViewSO> Hand = new();
    public List<CardViewSO> WeaponSlot = new();
    private void OnEnable()
    {
        GameEvents.HandChanged += UpdateHand;
        GameEvents.WeaponSlotChanged += UpdateWeapon;
    }

    private void OnDisable()
    {
        GameEvents.HandChanged -= UpdateHand;
        GameEvents.WeaponSlotChanged -= UpdateWeapon;
    }

    private void Start()
    {
        Hand = new List<CardViewSO>(_allCardViews);
        GameEvents.HandChanged?.Invoke(Hand);
    }

    private void UpdateHand(List<CardViewSO> list)
    {
        Hand = new List<CardViewSO>(list);
    }

    public void AddCardToHand(CardViewSO card)
    {
        Hand.Add(card);
        GameEvents.HandChanged?.Invoke(Hand);
    }
    private void UpdateWeapon(List<CardViewSO> list)
    {
        WeaponSlot = new List<CardViewSO>(list);
    }


}
