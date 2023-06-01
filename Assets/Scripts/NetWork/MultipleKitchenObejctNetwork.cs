using System.Collections.Generic;
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

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback;

        NetworkManager.Singleton.StartHost();
    }

    private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest,
        NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (TTKitchenGameManager.Instance.isGameStartState())
        {
            connectionApprovalResponse.Approved = true;
            connectionApprovalResponse.CreatePlayerObject = true;
        }
        else
        {
            connectionApprovalResponse.Approved = false;
        }
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenPoint)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndexInList(kitchenObjectSO), kitchenPoint.GetNetworkObject());
    }

    public void DestoryKitchenObject(KitchenObject kitchenObject)
    {
        DestoryKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex,
        NetworkObjectReference kitchenPointNetworkReference)
    {
        var kitchenObjectTransform =
            Instantiate(GetKitchenObjectSOFromIndex(kitchenObjectSOIndex).prefabTransform);

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
    }

    [ClientRpc]
    private void DestoryKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject networkObjectKitchenObject);
        var kitchenObject = networkObjectKitchenObject.GetComponent<KitchenObject>();

        kitchenObject.DestorySelf();

        kitchenObject.DestorySelfParent();
    }

    public int GetKitchenObjectSOIndexInList(KitchenObjectSO kitchenObjectSO)
    {
        return currentKitchenObjectSOList.KitchenObjectSOList.IndexOf(kitchenObjectSO);
    }

    public KitchenObjectSO GetKitchenObjectSOFromIndex(int kitchenObjectIndex)
    {
        return currentKitchenObjectSOList.KitchenObjectSOList[kitchenObjectIndex];
    }
}