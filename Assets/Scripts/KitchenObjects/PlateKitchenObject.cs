using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Rendering;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<ComplateVisualEvent> complateVisualEvent;
    public class ComplateVisualEvent
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    
    [SerializeField] private List<KitchenObjectSO> vaildKitchenObjectSOList;
    
    private List<KitchenObjectSO> kitchenObjectSOList;

    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!vaildKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Prevent repeated joining
            return false;
        }
        else
        {
            TryAddIngredientServerRpc(MultipleKitchenObejctNetwork.Instance.GetKitchenObjectSOIndexInList(kitchenObjectSO));
            
            return true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TryAddIngredientServerRpc(int kitchenObjectIndex)
    {
        TryAddIngredientClientRpc(kitchenObjectIndex);
    }

    [ClientRpc]
    private void TryAddIngredientClientRpc(int kitchenObjectIndex)
    {
        var kitchenObjectSO = MultipleKitchenObejctNetwork.Instance.GetKitchenObjectSOFromIndex(kitchenObjectIndex);
        
        kitchenObjectSOList.Add(kitchenObjectSO);
            
        complateVisualEvent?.Invoke(this,new ComplateVisualEvent()
        {
            kitchenObjectSO =  kitchenObjectSO
        });
    }

    public List<KitchenObjectSO> GetKitchenObjectList()
    {
        return kitchenObjectSOList;
    }
}
