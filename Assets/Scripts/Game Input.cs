using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnPauseAction;
    public event EventHandler OnJumpAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnCrouchAction;
    public event EventHandler OnScanObjectAction;
    public event EventHandler OnMorphObjectAction;

    public event EventHandler OnDisableMorphAction;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();
        playerInputActions.Debug.Enable();

        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Crouch.performed += Crouch_performed;
        playerInputActions.Player.ScanObject.performed += ScanObject_performed;
        playerInputActions.Player.MorphObject.performed += MorphObject_performed;

        playerInputActions.Debug.DisableMorph.performed += DisableMorph_performed;
    }

    private void DisableMorph_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDisableMorphAction?.Invoke(this, EventArgs.Empty);
    }

    private void MorphObject_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMorphObjectAction?.Invoke(this, EventArgs.Empty);
    }

    private void ScanObject_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnScanObjectAction?.Invoke(this, EventArgs.Empty);
    }

    private void Crouch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCrouchAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this.OnPauseAction, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        inputVector.Normalize();

        return inputVector;
    }
}
