using UnityEngine;

public class GarbageCounter : BaseCounter
{
    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject() && playerController.HasKitchenObject())
        {
            DestroyKitchenObject(playerController.GetKitchenObject());
        }
    }
}