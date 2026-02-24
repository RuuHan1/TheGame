using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardVisualizer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("CardData Referances")]
    [HideInInspector] public CardViewSO cardData;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private Image cardBgImage;
    [SerializeField] private Image cardFgImage;
    [Header("Card Action Settings")]
    [SerializeField] private float cardScaleSize = 1.2f;
    [SerializeField] private float hoverUpAmount = 100f;
    [SerializeField] private float hoverDelay = 0.1f;
    [HideInInspector] public Transform parentToReturnTo = null;
    [HideInInspector] public Transform placeholderParent = null;
    private GameObject placeholder = null;
    private CardPanel hoveredPanel;
    //
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private CardPanel currentSlot;
    [HideInInspector] private int slotIndex;
    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        UpdateHoveredPanel(eventData);
        UpdatePlaceholderPosition(eventData);
    }

    public void SetCurrentSlot(CardPanel slot)
    {
        currentSlot = slot;
    }
    public CardPanel GetCurrentSlot()
    {
        return currentSlot;
    }
    public void SetIndex(int i)
    {
        slotIndex = i;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        transform.localScale *= cardScaleSize;

        //transform.localPosition += Vector3.up* hoverUpAmount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        transform.localScale /= cardScaleSize;

        //transform.localPosition -= Vector3.up * hoverUpAmount;
    }

    public void Setup(CardViewSO cardData)
    {
        cardName.text = cardData.CardName;
        cardDescription.text = cardData.CardDescription;
        if (cardData.CardFgImage != null)
        {
            //cardBgImage.sprite = cardData.CardBgImage.sprite;
            cardFgImage.sprite = cardData.CardFgImage;
        }
    }
    private void CreatePlaceholder()
    {
        placeholder = new GameObject("Placeholder");
        RectTransform rect = placeholder.AddComponent<RectTransform>();
        LayoutElement layout = placeholder.AddComponent<LayoutElement>();

        LayoutElement myLayout = GetComponent<LayoutElement>();
        if (myLayout != null)
        {
            layout.preferredWidth = myLayout.preferredWidth;
            layout.preferredHeight = myLayout.preferredHeight;
        }

        placeholder.transform.SetParent(originalParent);
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }
    private void UpdateHoveredPanel(PointerEventData eventData)
    {
        hoveredPanel = null;

        foreach (var result in eventData.hovered)
        {
            CardPanel panel = result.GetComponent<CardPanel>();
            if (panel != null && panel.HasCapacity())
            {
                hoveredPanel = panel;
                break;
            }
        }
    }
    private void UpdatePlaceholderPosition(PointerEventData eventData)
    {
        if (hoveredPanel == null)
        {
            if (placeholder != null)
                placeholder.transform.SetParent(originalParent);

            return;
        }
        if (hoveredPanel == null || placeholder == null)
            return;
        placeholder.transform.SetParent(hoveredPanel.transform);

        for (int i = 0; i < hoveredPanel.transform.childCount; i++)
        {
            RectTransform child = hoveredPanel.transform.GetChild(i) as RectTransform;

            if (eventData.position.x < child.position.x)
            {
                placeholder.transform.SetSiblingIndex(i);
                return;
            }
        }

        placeholder.transform.SetSiblingIndex(hoveredPanel.transform.childCount);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(GetComponentInParent<Canvas>().transform, true);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == GetComponentInParent<Canvas>().transform)
        {
            ReturnToOriginalParent();
        }
    }

    public void ReturnToOriginalParent()
    {
        transform.SetParent(originalParent, false);
    }
}
