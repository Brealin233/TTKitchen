using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter,IWasVisualCounter
{
    public event EventHandler<IWasVisualCounter.counterVisualEventClass> counterVisualEvent;

    [SerializeField] private List<CuttingKitchenObjectSO> cuttingKitchenObjectSO;

    private float cuttingCount;

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                if (HasRecipeKitchenObject(playerController.GetKitchenObject()))
                {
                    cuttingCount = 0;
                    playerController.GetKitchenObject().SetKitchenObjectParent(this);
                    playerController.ClearKitchenObject();
                }
            }
        }
        else
        {
            if (!playerController.HasKitchenObject())
            {
                // todo: currently can move kitchenobject in UnSlice complete, and anim done with

                GetKitchenObject().SetKitchenObjectParent(playerController);
                counterVisualEvent?.Invoke(this,new IWasVisualCounter.counterVisualEventClass
                {
                    fillAmount = 0f
                });
                ClearKitchenObject();
            }
        }
    }

    public override void InteractAlternatePlayer(PlayerController playerController)
    {
        if (HasKitchenObject())
        {
            cuttingCount++;

            if (GetSliceKitchenObjectCountMax(GetKitchenObject()) != cuttingCount)
            {
                if (!playerController.HasKitchenObject() && HasRecipeKitchenObject(GetKitchenObject()))
                {
                    counterVisualEvent?.Invoke(this,new IWasVisualCounter.counterVisualEventClass
                    {
                        fillAmount = cuttingCount / GetSliceKitchenObjectCountMax(GetKitchenObject())
                    });
                }
            }
            else if(GetInputForOutputCuttingKitchenObject() && GetSliceKitchenObjectCountMax(GetKitchenObject()) == cuttingCount)
            {
                DestroyKitchenObject(GetKitchenObject());
                counterVisualEvent?.Invoke(this,new IWasVisualCounter.counterVisualEventClass
                {
                    fillAmount = cuttingCount / GetSliceKitchenObjectCountMax(GetKitchenObject())
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