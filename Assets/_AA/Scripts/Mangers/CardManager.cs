using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<CardViewSO> allCards = new();

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
        Hand = new List<CardViewSO>(allCards);
        GameEvents.HandChanged?.Invoke(Hand);
    }

    private void UpdateHand(List<CardViewSO> list)
    {
        Hand = new List<CardViewSO>(list);
    }

    private void UpdateWeapon(List<CardViewSO> list)
    {
        WeaponSlot = new List<CardViewSO>(list);
    }


}
