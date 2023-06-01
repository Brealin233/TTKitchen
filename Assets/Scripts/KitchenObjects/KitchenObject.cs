using System;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    private FollowTransform followTransform;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }
    
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenPointNetworkReference)
    {
        SetKitchenObjectParentClientRpc(kitchenPointNetworkReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenPointNetworkReference)
    {
        kitchenPointNetworkReference.TryGet(out NetworkObject networkKitchenObject);
        kitchenObjectParent = networkKitchenObject.GetComponent<IKitchenObjectParent>();
        
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        kitchenObjectParent.SetKitchenObject(this);

        followTransform.GetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public bool GetPlateKitchenObject(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    public void DestorySelfParent()
    {
        kitchenObjectParent.ClearKitchenObject();
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent kitchenPoint)
    {
        MultipleKitchenObejctNetwork.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenPoint);
    }

    public static void DestoryKitchenObject(KitchenObject kitchenObject)
    {
        MultipleKitchenObejctNetwork.Instance.DestoryKitchenObject(kitchenObject);
    }
}

