using System;
using Unity.Netcode;
using UnityEngine;

public class GarbageCounter : BaseCounter
{
    public static event EventHandler garbageSoundEvent;

    public static void ResetStaticData()
    {
        garbageSoundEvent = null;
    }
    
    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject() && playerController.HasKitchenObject())
        {
            KitchenObject.DestoryKitchenObject(playerController.GetKitchenObject());

            InteractPlayerServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractPlayerServerRpc()
    {
        InteractPlayerClientRpc();
    }

    [ClientRpc]
    private void InteractPlayerClientRpc()
    {
        garbageSoundEvent?.Invoke(this,EventArgs.Empty);
    }
}