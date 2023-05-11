using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameInputManager : MonoBehaviour
{
    public event EventHandler inputInteractHandler;
    public event EventHandler inputInteractAlternateHandler;

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = new InputManager();
        inputManager.Player.Enable();

        inputManager.Player.Interact.performed += InteractOnPerFormedEvent;
        inputManager.Player.InteractAlternate.performed += InteractAlternateOnPerformedEvent;
        inputManager.Player.Pause.performed += PauseOnPerformedEvent;
    }

    private void OnDestroy()
    {
        inputManager.Player.Interact.performed += InteractOnPerFormedEvent;
        inputManager.Player.InteractAlternate.performed += InteractAlternateOnPerformedEvent;
        inputManager.Player.Pause.performed += PauseOnPerformedEvent;
        
        inputManager.Dispose();
    }

    private void PauseOnPerformedEvent(InputAction.CallbackContext obj)
    {
        TTKitchenGameManager.Instance.SetGamePauseState();
    }

    private void InteractAlternateOnPerformedEvent(InputAction.CallbackContext obj)
    {
        inputInteractAlternateHandler?.Invoke(this, EventArgs.Empty);
    }

    private void InteractOnPerFormedEvent(InputAction.CallbackContext obj)
    {
        inputInteractHandler?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputManager.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}