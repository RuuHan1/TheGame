using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationUI : UIPanel
{
    [Header("Data")]
    [SerializeField] private RunDataSO _runCardData;

    [Header("Prefabs")]
    [SerializeField] private GameObject _cardPrefab; // Kart arayüz prefabý

    [Header("Stats References")]
    [SerializeField] private TextMeshProUGUI _killCountText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _levelText;

    [Header("Build References")]
    [SerializeField] private Transform _weaponCardsGrid;
    [SerializeField] private Transform _handCardsGrid;

    private void OnEnable()
    {
        UpdateStatsUI();
        UpdateCardsUI();
    }

    private void UpdateStatsUI()
    {
        // Not: Bu verilerin kaynaðý mevcut RunCardDataSO içinde yok. 
        // Ya SO geniþletilmeli ya da ilgili sistemden (örn: StatsManager.Instance) çekilmelidir.

        // Örnek kullaným:
        // _killCountText.text = _runCardData.killCount.ToString();
        // _timeText.text = FormatTime(_runCardData.survivalTime);
        // _levelText.text = _runCardData.levelReached.ToString();
    }

    private void UpdateCardsUI()
    {
        ClearGrid(_weaponCardsGrid);
        ClearGrid(_handCardsGrid);

        PopulateGrid(_runCardData.WeaponCards, _weaponCardsGrid);
        PopulateGrid(_runCardData.HandCards, _handCardsGrid);
    }

    private void ClearGrid(Transform grid)
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateGrid(List<CardViewSO> cards, Transform grid)
    {
        foreach (var cardData in cards)
        {
            GameObject newCard = Instantiate(_cardPrefab, grid);

            CardVisualizer visualizer = newCard.GetComponent<CardVisualizer>();
            if (visualizer != null)
            {
                visualizer.Setup(cardData);
            }
        }
    }
}