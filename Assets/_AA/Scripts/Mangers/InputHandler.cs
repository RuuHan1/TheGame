using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions _inputActions;

    private void OnEnable()
    {
        _inputActions.UI.Enable();
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMovePerformed;
        _inputActions.Player.Move.canceled += OnMovePerformed;
        _inputActions.Player.ToggleWeaponRangeCircle.performed += OnWeaponRangeBtnPerformed;
        _inputActions.Player.ToggleHandPanel.performed  += OnHandPanelBtnPerformed;
        _inputActions.Player.InteractAction.performed += OnInteractBtnPerformed;
        _inputActions.UI.Submit.performed += OnInteractBtnPerformedUI;
        GameEvents.TogglePlayerInput += OnTogglePlayerInput;
    }



    private void OnDisable()
    {
        _inputActions.UI.Disable();
        _inputActions.Player.Disable();
        _inputActions.Player.Move.performed -= OnMovePerformed;
        _inputActions.Player.Move.canceled -= OnMovePerformed;
        _inputActions.Player.ToggleWeaponRangeCircle.performed -= OnWeaponRangeBtnPerformed;
        _inputActions.Player.ToggleHandPanel.performed -= OnHandPanelBtnPerformed;
        _inputActions.Player.InteractAction.performed -= OnInteractBtnPerformed;
        _inputActions.UI.Submit.performed += OnInteractBtnPerformedUI;
        GameEvents.TogglePlayerInput += OnTogglePlayerInput;

    }

    private void OnTogglePlayerInput(bool obj)
    {
        if (obj) 
        {
            _inputActions.Player.Enable();
        }
        else
        {
            _inputActions.Player.Disable();
        }
    }

    private void OnInteractBtnPerformedUI(InputAction.CallbackContext context)
    {
        GameEvents.InteractUI?.Invoke();
        _inputActions.Player.Enable();
    }

    private void OnInteractBtnPerformed(InputAction.CallbackContext context)
    {
        GameEvents.Interact?.Invoke();
    }

    private void OnHandPanelBtnPerformed(InputAction.CallbackContext context)
    {
        GameEvents.ToggleCardPanel?.Invoke();
    }

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.UI.Cancel.performed += OnEscPerformed;
    }
    private void OnWeaponRangeBtnPerformed(InputAction.CallbackContext context)
    {
        GameEvents.ActivateWeaponRange_PlayerHud?.Invoke();
    }
    private void OnEscPerformed(InputAction.CallbackContext context)
    {
        GameEvents.TriggerEscapePressed();
    }


    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        GameEvents.TriggerMoveInput(context.ReadValue<Vector2>());
    }

}
