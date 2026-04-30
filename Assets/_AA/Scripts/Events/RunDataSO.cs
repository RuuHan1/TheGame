using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRunData", menuName = "Game Data/Run Data")]
public class RunDataSO : ScriptableObject
{
    [HideInInspector]public List<CardViewSO> WeaponCards = new();
    [HideInInspector]public List<CardViewSO> HandCards = new();
    [Header("Run Stats Data")]
    public int killCount;
    public float survivalTime;
    public int levelReached;

    public void ResetData()
    {
        WeaponCards.Clear();
        HandCards.Clear();

        killCount = 0;
        survivalTime = 0f;
        levelReached = 1;
    }
}