using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private List<CuttingKitchenObjectSO> cuttingKitchenObjectSO;

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                if (HasRecipeKitchenObject(playerController.GetKitchenObject()))
                {
                    playerController.GetKitchenObject().SetKitchenObjectParent(this);
                    playerController.ClearKitchenObject();
                }
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
            if (!playerController.HasKitchenObject() && HasRecipeKitchenObject(GetKitchenObject()))
            {
                DestroyKitchenObject(GetKitchenObject());
                KitchenObject.SpawnKitchenObject(GetInputForOutputCuttingKitchenObject().GetKitchenObjectSO(), this);
            }
        }
    }

    private bool HasRecipeKitchenObject(KitchenObject kitchenObject)
    {
        foreach (CuttingKitchenObjectSO kitchenObjectSO in cuttingKitchenObjectSO)
        {
            if (kitchenObjectSO.inputKitchenObject.GetKitchenObjectSO() == kitchenObject.GetKitchenObjectSO())
            {
                return true;
            }
        }

        return false;
    }

    private KitchenObject GetInputForOutputCuttingKitchenObject()
    {
        foreach (CuttingKitchenObjectSO kitchenObjectSO in cuttingKitchenObjectSO)
        {
            if (kitchenObjectSO.inputKitchenObject.GetKitchenObjectSO() == GetKitchenObject().GetKitchenObjectSO())
            {
                return kitchenObjectSO.outputKitchenObject;
            }
        }

        return null;
    }
}