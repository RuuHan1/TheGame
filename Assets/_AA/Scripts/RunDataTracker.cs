using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunDataTracker : MonoBehaviour
{
    [SerializeField] private RunDataSO _runData;

    private void OnEnable()
    {
        GameEvents.PlayerLevelUp += OnPlayerLevelUp;
        GameEvents.SecondPassed += OnSecondPass;
        GameEvents.EnemyDied += UpdateKillCount;

        GameEvents.HandChanged += UpdateHandCardsData;
        GameEvents.WeaponSlotChanged += UpdateWeaponCardsData;
    }

    private void OnDisable()
    {
        GameEvents.PlayerLevelUp -= OnPlayerLevelUp;
        GameEvents.SecondPassed -= OnSecondPass;
        GameEvents.EnemyDied -= UpdateKillCount;

        GameEvents.HandChanged -= UpdateHandCardsData;
        GameEvents.WeaponSlotChanged -= UpdateWeaponCardsData;
    }
    private void Start()
    {
        _runData.ResetData();
    }
    private void OnSecondPass(int timer)
    {
        _runData.SurvivalTime = timer;
    }

    private void OnPlayerLevelUp(Transform transform,int level)
    {
        _runData.LevelReached = level;
    }

    private void UpdateKillCount(int obj)
    {
        _runData.KillCount++;
    }

    private void UpdateHandCardsData(List<CardViewSO> cards)
    {
        _runData.HandCards.Clear();
        _runData.HandCards.AddRange(cards);
    }

    private void UpdateWeaponCardsData(List<CardViewSO> cards)
    {
        _runData.WeaponCards.Clear();
        _runData.WeaponCards.AddRange(cards);
    }
}