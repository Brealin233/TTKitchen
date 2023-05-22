using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler handleGrabbedObject;
    [SerializeField] private KitchenObjectSO initPrefab;

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (!playerController.HasKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(initPrefab, playerController);
                
                InteractPlayerServerRpc();
            }
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
        handleGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}