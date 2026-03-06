using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents 
{
    public static Action<Vector3, float, bool> OnEnemyDamaged;
    public static Action<int> EnemyDied;
    public static Action<Transform> PlayerPosition;
    public static Action<Vector3, float> EnemyDiedXp;
    public static Action<float> XpCollected;
    public static Action<Transform> PlayerLevelUp;
    public static Action<float> PlayerDamaged;
    public static Action<float,float> PlayerHealthChanged;
    public static Action<List<CardViewSO>> HandChanged;
    public static Action<List<CardViewSO>> WeaponSlotChanged;
    public static Action<bool> GameStatesChanged;
    public static Action<int> WeaponChanged;
    public static Action SlotMachineTaken;
    public static Action<CardViewSO> CardAwarded;
    //UI
    //xp ve level icin
    public static Action<float, float,int> PlayerXpChanged;
    //timer icin
    public static Action SecondPassed;
    //weapon slot sayisini degistiren kart kullaninca
    public static Action<int> WeaponSlotCountChanged;
}
