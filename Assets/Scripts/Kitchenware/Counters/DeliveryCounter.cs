using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void InteractPlayer(PlayerController playerController)
    {
        if (playerController.HasKitchenObject())
        {
            if (playerController.GetKitchenObject().GetPlateKitchenObject(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliveryRecipe(plateKitchenObject);
                DestroyKitchenObject(plateKitchenObject);
            }
        }
    }
}
