using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private InputActionReference _InputActions;

    private void OnEnable()
    {
        _InputActions.action.performed += OnGamePaused;
    }
    private void OnDisable()
    {
        _InputActions.action.performed -= OnGamePaused;
    }
    private void OnGamePaused(InputAction.CallbackContext context)
    {
        bool isPaused = _pausePanel.activeSelf;
        _pausePanel.SetActive(!isPaused);
        GameEvents.GamePaused?.Invoke(!isPaused);
    }
}
