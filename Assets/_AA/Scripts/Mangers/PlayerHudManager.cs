using Lean.Pool;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHudManager : UIPanel
{
    [SerializeField] private InputActionReference _inputReference;
    [SerializeField] private GameObject _worldText;
    private Transform _playerTransform;

    private void OnEnable()
    {
        _inputReference.action.Enable();
        _inputReference.action.performed += ToggleWeaponRange;
        GameEvents.PlayerDamaged += OnPlayerDamaged;
        GameEvents.PlayerPosition += OnPlayerTransformAssigned;
        GameEvents.PlayerHealthRegen_PlayerStats += OnPlayerHealthRegen;
    }

   

    private void OnDisable()
    {
        _inputReference.action.Disable();
        _inputReference.action.performed -= ToggleWeaponRange;
        GameEvents.PlayerDamaged -= OnPlayerDamaged;
        GameEvents.PlayerPosition -= OnPlayerTransformAssigned;
        GameEvents.PlayerHealthRegen_PlayerStats -= OnPlayerHealthRegen;
    }

    private void ToggleWeaponRange(InputAction.CallbackContext context)
    {
        GameEvents.ActivateWeaponRange_PlayerHud?.Invoke();
    }
    private void OnPlayerHealthRegen(float obj)
    {
        Vector2 spawnPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y, 0) + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f), 0);
        GameObject healthText = LeanPool.Spawn(_worldText, spawnPos, Quaternion.identity);

        healthText.GetComponent<DamageText>().OnPlayerRegen(obj);
    }

    private void OnPlayerTransformAssigned(Transform transform)
    {
        _playerTransform = transform;
    }

    private void OnPlayerDamaged(float obj)
    {
        Vector2 spawnPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y, 0) + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f), 0);
        GameObject damageText = LeanPool.Spawn(_worldText, spawnPos, Quaternion.identity);

        damageText.GetComponent<DamageText>().Initialize(obj,Color.red);
    }
}
