using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<counterVisualEventClass> counterVisualEvent;

    public class counterVisualEventClass : EventArgs
    {
        public float fillAmount;
    }

    [SerializeField] private List<CuttingKitchenObjectSO> cuttingKitchenObjectSO;

    private float cuttingCount;

    public override void InteractPlayer(PlayerController playerController)
    {
        // todo: currently can move kitchenobject in UnSlice complete  
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                if (HasRecipeKitchenObject(playerController.GetKitchenObject()))
                {
                    cuttingCount = 1;
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
            if (GetSliceKitchenObjectCountMax(GetKitchenObject()) != cuttingCount)
            {
                if (!playerController.HasKitchenObject() && HasRecipeKitchenObject(GetKitchenObject()))
                {
                    cuttingCount++;
                    counterVisualEvent?.Invoke(this,new counterVisualEventClass()
                    {
                        fillAmount = cuttingCount
                    });
                }
            }
            else if(GetInputForOutputCuttingKitchenObject())
            {
                DestroyKitchenObject(GetKitchenObject());
                counterVisualEvent?.Invoke(this,new counterVisualEventClass()
                {
                    fillAmount = cuttingCount + 1
                });
                KitchenObject.SpawnKitchenObject(GetInputForOutputCuttingKitchenObject().GetKitchenObjectSO(), this);
            }
        }
    }

    private float GetSliceKitchenObjectCountMax(KitchenObject kitchenObject)
    {
        foreach (CuttingKitchenObjectSO kitchenObjectSO in cuttingKitchenObjectSO)
        {
            if (kitchenObjectSO.inputKitchenObject.GetKitchenObjectSO() == kitchenObject.GetKitchenObjectSO())
            {
                return kitchenObjectSO.inputKitchenObject.GetKitchenObjectSO().cuttingTimeMax;
            }
        }

        return 0;
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