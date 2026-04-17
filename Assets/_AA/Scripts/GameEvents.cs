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
    public static Action<bool> GamePaused;
    public static Action<int> WeaponChanged;
    public static Action SlotMachineTaken;
    public static Action<CardViewSO> CardAwarded;
    //UI
    public static Action ActivateWeaponRange_PlayerHud;
    public static Action<CardType[],CardViewSO> WhellSpinned_SlothMachineManager;
    public static Action NewRunClicked_UIManager;
    //regen icin,PlayerHealthChanged kullanamiyorum cunku Ondan current healtimi gondermem gerekiyor.
    public static Action<float> PlayerHealthRegen_PlayerStats; 
    //xp ve level icin
    public static Action<float, float,int> PlayerXpChanged;
    //timer icin
    public static Action SecondPassed;
    //weapon slot sayisini degistiren kart kullaninca
    public static Action<int> WeaponSlotCountChanged;
    //vfx icin
    public static Action<VFXType,Vector2> PlayVFX_Projectile;
    public static Action<VFXType,Vector2> PlayVFX_Enemy;
    //Camera
    public static Action<float> ShakeCamera_EnemyManager;
    //GameMAnager
    public static Action DecreaseTimeScale_EnemyManager;
    public static Action SpawnBoss_GameManager;
    //EnemySword
    public static Action EnemySwordHit;
    //
    public static Action<string,bool> PopUpInfoPanel;
    public static Action GameFinished_BossSpawner;
    //
    public static Action BossDefeated_Boss;
    //sound
    public static Action<SfxType> PlaySound;
}
