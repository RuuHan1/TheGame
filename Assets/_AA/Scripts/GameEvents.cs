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
    public static Action<Transform,int> PlayerLevelUp;
    public static Action<float> PlayerDamaged;
    public static Action<float,float> PlayerHealthChanged;
    public static Action<List<CardViewSO>> HandChanged;
    public static Action<List<CardViewSO>> WeaponSlotChanged;
    public static Action<bool> GamePaused;
    public static Action<int> WeaponChanged;
    public static Action SlotMachineTaken;
    public static Action<CardViewSO> CardAwarded;
    public static Action PlayerDied;
    public static Action<bool> TogglePlayerInput;
    public static Action<bool> ToggleUIInput;
    //UI
    public static Action ActivateWeaponRange_PlayerHud;
    public static Action<CardType[],CardViewSO> WhellSpinned_SlothMachineManager;
    public static Action NewRunClicked_UIManager;
    public static Action<float> SfxSliderChanged;
    public static Action<float> MusicSliderChanged;
    public static Action<UIPanel> AddPanelToStack;
    public static Action EscapePressed;
    public static Action<int> PlayButtonClicked;
    public static Action<float> LoadingProgressUpdated_LoadingState;
    public static Action<List<CardViewSO>> EndGameWeaponCardsReceived_CardPanel;
    public static Action<List<CardViewSO>> EndGameHandCardsReceived_CardPanel;
    public static Action ToggleCardPanel;
    public static Action Interact;
    public static Action InteractUI;
    public static Action GoMainMenu;

    //regen icin,PlayerHealthChanged kullanamiyorum cunku Ondan current healtimi gondermem gerekiyor.
    public static Action<float> PlayerHealthRegen_PlayerStats; 
    //xp ve level icin
    public static Action<float, float,int> PlayerXpChanged;
    //timer icin
    public static Action<int> SecondPassed;
    //weapon slot sayisini degistiren kart kullaninca
    public static Action<int> WeaponSlotCountChanged;
    //vfx icin
    public static Action<VFXType,Vector2> PlayVFX_Projectile;
    public static Action<VFXType,Vector2> PlayVFX_Enemy;
    //Camera
    public static Action<float> ShakeCamera_EnemyManager;
    //GameManager
    public static Action DecreaseTimeScale_EnemyManager;
    public static Action SpawnBoss_GameManager;
    //EnemySword
    public static Action EnemySwordHit;
    //
    public static Action<string,bool> ToggleInfoPanel;
    public static Action GameFinished_BossSpawner;
    //
    public static Action BossDefeated_Boss;
    //sound
    public static Action<SfxType> PlaySound;
    public static event Action<MusicType> PlayMusic;
    public static event Action<bool> StopMusic; // bool = fade?
    public static event Action<Vector2> OnMoveInput;
    public static void TriggerPlaySound(SfxType type) => PlaySound?.Invoke(type);
    public static void TriggerPlayMusic(MusicType type) => PlayMusic?.Invoke(type);
    public static void TriggerStopMusic(bool fade = true) => StopMusic?.Invoke(fade);
    public static void TriggerEscapePressed() => EscapePressed?.Invoke();
    public static void TriggerMoveInput(Vector2 input) => OnMoveInput?.Invoke(input);
}
