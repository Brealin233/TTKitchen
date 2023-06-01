using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public static event EventHandler dropDownSoundEvent;
    public static void ResetStaticData()
    {
        dropDownSoundEvent = null;
    }
    
    [SerializeField] private KitchenObjectSO initPrefab;

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                dropDownSoundEvent?.Invoke(this,EventArgs.Empty);
                
                //KitchenObject.SpawnKitchenObject(playerController.GetKitchenObject().GetKitchenObjectSO(),this);
                playerController.GetKitchenObject().SetKitchenObjectParent(this);
                playerController.ClearKitchenObject();
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
                        KitchenObject.DestoryKitchenObject(GetKitchenObject());
                    }
                }
                else
                {
                    if (GetKitchenObject().GetPlateKitchenObject(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(playerController.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestoryKitchenObject(playerController.GetKitchenObject());
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