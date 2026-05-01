using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationUI : UIPanel
{
    [Header("Data")]
    [SerializeField] private RunDataSO _runData;

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
        _killCountText.text = _runData.KillCount.ToString();
        _levelText.text = _runData.LevelReached.ToString();
        float time = _runData.SurvivalTime;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateCardsUI()
    {
        ClearGrid(_weaponCardsGrid);
        ClearGrid(_handCardsGrid);

        PopulateGrid(_runData.WeaponCards, _weaponCardsGrid);
        PopulateGrid(_runData.HandCards, _handCardsGrid);
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
    public void OnMainMenuBtnClick()
    {
        GameEvents.GoMainMenu?.Invoke();
    }
}