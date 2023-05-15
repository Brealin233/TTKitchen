using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameInputManager : MonoBehaviour
{
    // todo: add default settings
    public static GameInputManager Instance { get; private set; } 
    
    public enum Bindings
    {
        Move_UP,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        pause
    }
    
    public event EventHandler inputInteractHandler;
    public event EventHandler inputInteractAlternateHandler;

    private InputManager inputManager;

    private const string PLAYER_PREFS_BINDINGMAP = "PLAYER_PREFS_BINDINGMAP";

    private void Awake()
    {
        Instance = this;
        
        inputManager = new InputManager();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGMAP))
        {
            inputManager.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGMAP));
        }
        
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
    
    public string GetBindingsText(Bindings bindings)
    {
        switch (bindings)
        {
            default:
            case Bindings.Move_UP:
                return inputManager.Player.Move.bindings[1].ToDisplayString();
            case Bindings.Move_Down:
                return inputManager.Player.Move.bindings[2].ToDisplayString();
            case Bindings.Move_Left:
                return inputManager.Player.Move.bindings[3].ToDisplayString();
            case Bindings.Move_Right:
                return inputManager.Player.Move.bindings[4].ToDisplayString();
            case Bindings.InteractAlternate:
                return inputManager.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Bindings.Interact:
                return inputManager.Player.Interact.bindings[0].ToDisplayString();
            case Bindings.pause:
                return inputManager.Player.Pause.bindings[0].ToDisplayString();
        }   
    }

    public void RebindBindings(Bindings bindings, Action rebound)
    {
        inputManager.Player.Disable();

        InputAction inputAction;
        int index;

        switch (bindings)
        {
            default:
            case Bindings.Move_UP:
                inputAction = inputManager.Player.Move;
                index = 1;
                break;
            case Bindings.Move_Down:
                inputAction = inputManager.Player.Move;
                index = 2;
                break;
            case Bindings.Move_Left:
                inputAction = inputManager.Player.Move;
                index = 3;
                break;
            case Bindings.Move_Right:
                inputAction = inputManager.Player.Move;
                index = 4;
                break;
            case Bindings.Interact:
                inputAction = inputManager.Player.Interact;
                index = 0;
                break;
            case Bindings.InteractAlternate:
                inputAction = inputManager.Player.InteractAlternate;
                index = 0;
                break;
            case Bindings.pause:
                inputAction = inputManager.Player.Pause;
                index = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(index).OnComplete(callback =>
        {
            inputManager.Player.Disable();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGMAP,inputAction.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            
            rebound();
            inputManager.Player.Enable();
        }).Start();
    }
}