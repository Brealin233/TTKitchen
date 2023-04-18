using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameInputManager : MonoBehaviour
{
   private InputManager inputManager;

   private void Awake()
   {
      inputManager = new InputManager();
      inputManager.Player.Enable();
   }

   public Vector2 GetMovementVectorNormalized()
   {
      Vector2 inputVector = inputManager.Player.Move.ReadValue<Vector2>(); 

      return inputVector.normalized;
   }
}
