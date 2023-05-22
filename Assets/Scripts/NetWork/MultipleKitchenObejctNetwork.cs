using Unity.Netcode;
using UnityEngine;

public class MultipleKitchenObejctNetwork : NetworkBehaviour
{
    public static MultipleKitchenObejctNetwork Instance { get; private set; }

    [SerializeField] private KitchenObjectListSO currentKitchenObjectSOList;

    private void Awake()
    {
        Instance = this;
    }
    
    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent kitchenPoint)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndexInList(kitchenObjectSO),kitchenPoint.GetNetworkObject());
    }

    public void DestoryKitchenObject(KitchenObject kitchenObject)
    {
        DestoryKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex,NetworkObjectReference kitchenPointNetworkReference)
    {
        var kitchenObjectTransform =
            Instantiate(GetKitchenObjectFromIndex(kitchenObjectSOIndex).prefabTransform);

        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);

        kitchenPointNetworkReference.TryGet(out NetworkObject kitchenNetworkObject);
        var kitchenObjectParent = kitchenNetworkObject.GetComponent<IKitchenObjectParent>();
        
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(kitchenObjectParent);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestoryKitchenObjectServerRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject networkObjectKitchenObject);
        var kitchenObject = networkObjectKitchenObject.GetComponent<KitchenObject>();
        
        DestoryKitchenObjectParentClientRpc(kitchenObjectReference);
        
        kitchenObject.DestorySelf();
    }

    [ClientRpc]
    private void DestoryKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectReference)
    {
        
        kitchenObjectReference.TryGet(out NetworkObject networkObjectKitchenObject);
        var kitchenObject = networkObjectKitchenObject.GetComponent<KitchenObject>();
        
        kitchenObject.DestorySelfParent();
    }

    private int GetKitchenObjectSOIndexInList(KitchenObjectSO kitchenObjectSO)
    {
        return currentKitchenObjectSOList.KitchenObjectSOList.IndexOf(kitchenObjectSO);
    }

    private KitchenObjectSO GetKitchenObjectFromIndex(int kitchenObjectIndex)
    {
        return currentKitchenObjectSOList.KitchenObjectSOList[kitchenObjectIndex];
    }
}
