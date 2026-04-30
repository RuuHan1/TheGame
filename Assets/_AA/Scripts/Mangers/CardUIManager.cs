using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardUIManager : UIPanel
{
    [SerializeField] private GameObject _handPanel;
    //[SerializeField] private GameObject _handPanelBg;
    [SerializeField] private GameObject _weaponSlotPanel;
    [SerializeField] private GameObject cardPrefab;

    private void OnEnable()
    {
        GameEvents.HandChanged += InitializeHand;
        GameEvents.ToggleCardPanel += OnToggleCardPanel;
    }

    private void OnDisable()
    {
        GameEvents.HandChanged -= InitializeHand;
        GameEvents.ToggleCardPanel -= OnToggleCardPanel;
    }

    private void OnToggleCardPanel()
    {
        bool active = _handPanel.activeSelf;
        GameEvents.GamePaused?.Invoke(!active);
        _handPanel.SetActive(!active);
        _weaponSlotPanel.SetActive(!active);
    }

    private void InitializeHand(List<CardViewSO> cards)
    {
        foreach (Transform t in _handPanel.transform)
            Destroy(t.gameObject);

        foreach (var card in cards)
        {
            GameObject go = Instantiate(cardPrefab, _handPanel.transform);
            go.transform.localScale = Vector3.one;

            CardVisualizer visual = go.GetComponent<CardVisualizer>();
            visual.cardData = card;
            visual.Setup(card);
            
            CardPanel panel = _handPanel.GetComponent<CardPanel>();
            if (panel != null)
            {
                visual.SetCurrentSlot(panel);
            }
        }
    }
    
}
