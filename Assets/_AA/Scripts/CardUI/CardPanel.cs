using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPanel : MonoBehaviour, IDropHandler
{
    private int maxSlot;
    [SerializeField] private bool isWeaponSlot = false;
    private List<CardVisualizer> cardsOnList = new();
    [Header("Events")]
    [SerializeField] private WeaponState weaponState;
    [SerializeField] private WeaponChangedEvent weaponChangedEvent;

    private void OnEnable()
    {
        weaponChangedEvent.OnEventRaised += OnWeaponChanged;
        OnWeaponChanged(weaponState.CurrentSlot);
    }

    private void OnWeaponChanged(int obj)
    {
        maxSlot = weaponState.CurrentSlot;
    }

    private void OnDisable()
    {
        weaponChangedEvent.OnEventRaised -= OnWeaponChanged;
    }
    public void NotifyCardRemoved(CardVisualizer card)
    {
        RecalculateOrder();
    }
    public void OnDrop(PointerEventData eventData)
    {
        CardVisualizer card = eventData.pointerDrag?.GetComponent<CardVisualizer>();
        if (card == null)
            return;

        if (!HasCapacity() && isWeaponSlot)
        {
            card.ReturnToOriginalParent();
            return;
        }

        AddCard(card, eventData);
    }

    public bool HasCapacity()
    {
        return transform.childCount < maxSlot;
    }

    private void AddCard(CardVisualizer card, PointerEventData eventData)
    {
        card.transform.SetParent(transform, false);

        int index = GetDropIndex(eventData);
        card.transform.SetSiblingIndex(index);

        card.SetCurrentSlot(this);

        RecalculateOrder();
    }

    private void RecalculateOrder()
    {
        cardsOnList.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            CardVisualizer card = transform.GetChild(i).GetComponent<CardVisualizer>();
            if (card != null)
            {
                cardsOnList.Add(card);
                card.SetIndex(i);
            }
        }

        SendEvent();
    }

    private void SendEvent()
    {
        //calismassa lsiteyi new leyip dene
        List<CardViewSO> data = GetCurrentCardData();

        if (isWeaponSlot)
        {
            GameEvents.WeaponSlotChanged?.Invoke(data);
        }
        else
        {
            GameEvents.HandChanged?.Invoke(data);
        }
    }

    private int GetDropIndex(PointerEventData eventData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i) as RectTransform;

            if (eventData.position.x < child.position.x)
                return i;
        }

        return transform.childCount;
    }
    private List<CardViewSO> GetCurrentCardData()
    {
        List<CardViewSO> data = new();
        foreach (var card in cardsOnList)
        {
            data.Add(card.cardData);
        }
        return data;
    }

    
}
