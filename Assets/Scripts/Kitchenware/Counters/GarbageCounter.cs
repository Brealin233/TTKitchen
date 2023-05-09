using System;
using UnityEngine;

public class GarbageCounter : BaseCounter
{
    public static event EventHandler garbageSoundEvent;
    
    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject() && playerController.HasKitchenObject())
        {
            garbageSoundEvent?.Invoke(this,EventArgs.Empty);
            DestroyKitchenObject(playerController.GetKitchenObject());
        }
    }
}