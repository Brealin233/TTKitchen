using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSliceSO;

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                playerController.GetKitchenObject().SetKitchenObjectParent(this);
                playerController.ClearKitchenObject();
            }
        }
        else 
        {
            if (!playerController.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
                ClearKitchenObject();
            }
        }
    }

    public override void InteractAlternatePlayer(PlayerController playerController)
    {
        if (HasKitchenObject())
        {
            DestroyKitchenObject(GetKitchenObject());

            if (!playerController.HasKitchenObject())
            {
               KitchenObject.SpawnKitchenObject(kitchenObjectSliceSO,this);
            }
        }
    }
}