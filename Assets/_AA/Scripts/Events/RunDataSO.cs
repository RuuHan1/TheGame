using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRunData", menuName = "Game Data/Run Data")]
public class RunDataSO : ScriptableObject
{
    [HideInInspector]public List<CardViewSO> WeaponCards = new();
    [HideInInspector]public List<CardViewSO> HandCards = new();
    [Header("Run Stats Data")]
    [HideInInspector] public int KillCount;
    [HideInInspector] public float SurvivalTime;
    [HideInInspector] public int LevelReached;

    public void ResetData()
    {
        WeaponCards.Clear();
        HandCards.Clear();

        KillCount = 0;
        SurvivalTime = 0f;
        LevelReached = 1;
    }
}