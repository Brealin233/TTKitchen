using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO initPrefab;

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                playerController.GetKitchenObject().SetKitchenObjectParent(this);
                playerController.ClearKitchenObject();
            }
            else
            {
                // counter nothing，player nothing
            }
        }
        else
        {
            if (playerController.HasKitchenObject())
            {
                if (playerController.GetKitchenObject().GetPlateKitchenObject(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        DestroyKitchenObject(GetKitchenObject());
                    }
                }
                else
                {
                    if (GetKitchenObject().GetPlateKitchenObject(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(playerController.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            DestroyKitchenObject(playerController.GetKitchenObject());
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
                ClearKitchenObject();
            }
        }
    }
}