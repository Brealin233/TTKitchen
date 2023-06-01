using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter, IWasVisualCounter
{
    public static event EventHandler cuttingsSoundEvent;

    public static void ResetStaticData()
    {
        cuttingsSoundEvent = null;
    }

    public event EventHandler<IWasVisualCounter.counterVisualEventClass> counterVisualEvent;

    [SerializeField] private List<CuttingKitchenObjectSO> cuttingKitchenObjectSO;

    private float cuttingCount;
    private KitchenObject kitchenObject;

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                if (HasRecipeKitchenObject(playerController.GetKitchenObject()))
                {
                    kitchenObject = playerController.GetKitchenObject();

                    kitchenObject.SetKitchenObjectParent(this);

                    InteractServerRpc();
                }
            }
        }
        else
        {
            if (playerController.HasKitchenObject())
            {
                if (playerController.GetKitchenObject()
                    .GetPlateKitchenObject(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestoryKitchenObject(GetKitchenObject());
                    }
                }
            }
            else
            {
                // todo: currently can move kitchenobject in UnSlice complete, and anim done with

                GetKitchenObject().SetKitchenObjectParent(playerController);
                ClearKitchenObjectServerRpc();
                counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass
                {
                    fillAmount = 0f
                });
            }
        }
    }

    public override void InteractAlternatePlayer(PlayerController playerController)
    {
        if (HasKitchenObject())
        {
            InteractAlternateServerRpc(playerController.NetworkObject);
            CuttingInteractAlternateServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractServerRpc()
    {
        InteractClientRpc();
    }

    [ClientRpc]
    private void InteractClientRpc()
    {
        cuttingCount = 0;
        cuttingsSoundEvent?.Invoke(this, EventArgs.Empty);

        counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass
        {
            fillAmount = 0
        });
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClearKitchenObjectServerRpc()
    {
        ClearKitchenObjectClientRpc();
    }

    [ClientRpc]
    private void ClearKitchenObjectClientRpc()
    {
        ClearKitchenObject();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractAlternateServerRpc(NetworkObjectReference networkObjectReference)
    {
        InteractAlternateClientRpc(networkObjectReference);
    }

    [ClientRpc]
    private void InteractAlternateClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject networkObject);
        var playerController = networkObject.GetComponent<IKitchenObjectParent>();

        cuttingCount++;

        if (HasRecipeKitchenObject(GetKitchenObject()))
        {
            cuttingsSoundEvent?.Invoke(this, EventArgs.Empty);
            counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass
            {
                fillAmount = cuttingCount / GetSliceKitchenObjectCountMax(GetKitchenObject())
            });
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CuttingInteractAlternateServerRpc()
    {
        if (GetInputForOutputCuttingKitchenObject(GetKitchenObject()) &&
            GetSliceKitchenObjectCountMax(GetKitchenObject()) == cuttingCount)
        {
            cuttingsSoundEvent?.Invoke(this, EventArgs.Empty);

            var outputKitchenObject = GetInputForOutputCuttingKitchenObject(GetKitchenObject()); 
            
            KitchenObject.DestoryKitchenObject(GetKitchenObject());

            KitchenObject.SpawnKitchenObject(outputKitchenObject.GetKitchenObjectSO(),
                this);
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

    private KitchenObject GetInputForOutputCuttingKitchenObject(KitchenObject kitchenObject)
    {
        foreach (CuttingKitchenObjectSO kitchenObjectSO in cuttingKitchenObjectSO)
        {
            if (kitchenObjectSO.inputKitchenObject.GetKitchenObjectSO() == kitchenObject.GetKitchenObjectSO())
            {
                return kitchenObjectSO.outputKitchenObject;
            }
        }

        return null;
    }
}