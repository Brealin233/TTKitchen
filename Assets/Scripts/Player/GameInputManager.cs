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

        inputManager.Player.Interact.performed += InteractOnPerFormed;
        inputManager.Player.InteractAlternate.performed += InteractAlternateOnPerformed;
    }

    private void InteractAlternateOnPerformed(InputAction.CallbackContext obj)
    {
        inputInteractAlternateHandler?.Invoke(this, EventArgs.Empty);
    }

    private void InteractOnPerFormed(InputAction.CallbackContext obj)
    {
        inputInteractHandler?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputManager.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}