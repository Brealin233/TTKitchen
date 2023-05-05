using System;
using System.Collections;
using System.Collections.Generic;
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
                handleGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}