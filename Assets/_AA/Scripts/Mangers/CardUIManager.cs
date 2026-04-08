using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardUIManager : MonoBehaviour
{
    [SerializeField] private GameObject handPanel;
    //[SerializeField] private GameObject _handPanelBg;
    [SerializeField] private GameObject weaponSlotPanel;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private InputActionReference inputReference;

    private void OnEnable()
    {
        GameEvents.HandChanged += InitializeHand;
        inputReference.action.Enable();
        inputReference.action.performed += TogglePanels;

    }

    private void OnDisable()
    {
        GameEvents.HandChanged -= InitializeHand;
        inputReference.action.Disable();
        inputReference.action.performed -= TogglePanels;
    }

    private void Start()
    {
        
    }
    private void TogglePanels(InputAction.CallbackContext context)
    {
        bool active = handPanel.activeSelf;
        GameEvents.GamePaused?.Invoke(!active);
        handPanel.SetActive(!active);
        weaponSlotPanel.SetActive(!active);
        //_handPanelBg.SetActive(!active);
    }

    private void InitializeHand(List<CardViewSO> cards)
    {
        foreach (Transform t in handPanel.transform)
            Destroy(t.gameObject);

        foreach (var card in cards)
        {
            GameObject go = Instantiate(cardPrefab, handPanel.transform);
            go.transform.localScale = Vector3.one;

            CardVisualizer visual = go.GetComponent<CardVisualizer>();
            visual.cardData = card;
            visual.Setup(card);
            
            CardPanel panel = handPanel.GetComponent<CardPanel>();
            if (panel != null)
            {
                visual.SetCurrentSlot(panel);
            }
        }
    }
    
}
