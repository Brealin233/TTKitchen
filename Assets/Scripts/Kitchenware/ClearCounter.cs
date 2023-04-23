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
                Debug.Log("V");
            }
        }
        else 
        {
            if (playerController.HasKitchenObject())
            {
                Debug.Log("VAR");
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
                ClearKitchenObject();
            }
        }
    }
}