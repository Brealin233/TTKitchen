using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : NetworkBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler deliverySpawnEvent;
    public event EventHandler deliveryDisableEvent;
    public event EventHandler deliverySuccessEvent;
    public event EventHandler deliveryFaildEvent;
    public event EventHandler deliverySuccessBoardEvent;
    public event EventHandler deliveryFailedBoardEvent;
    public event EventHandler deliveryAnimEvent;


    [SerializeField] private RecipeListSO recipeList;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTime = 3f;
    private readonly float spawnRecipeTimeMax = 4f;
    private int spawnRecipeCount;
    private readonly int spawnRecipeCountMax = 4;

    private int deliveredCount;

    private void Awake()
    {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        spawnRecipeTime -= Time.deltaTime;

        if (TTKitchenGameManager.Instance.IsInGameState())
        {
            if (spawnRecipeTime < 0f)
            {
                spawnRecipeTime = spawnRecipeTimeMax;

                if (spawnRecipeCount < spawnRecipeCountMax)
                {
                    var waitingRecipeSOIndex = Random.Range(0, recipeList.RecipeListSOList.Count);

                    spawnRecipeCount++;
                    SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
                }
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        waitingRecipeSOList.Add(recipeList.RecipeListSOList[waitingRecipeSOIndex]);

        deliverySpawnEvent?.Invoke(this, EventArgs.Empty);
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        // foreach (RecipeSO waitingRecipeSO in waitingRecipeSOList)
        // {
        //     bool matchRecipe = true;
        //
        //     // has same recipe number
        //     if (waitingRecipeSO.recipeSOList.Count == plateKitchenObject.GetKitchenObjectList().Count)
        //     {
        //         bool hasSameRecipe = false;
        //
        //         // Compare whether there are the same recipe
        //         foreach (KitchenObjectSO recipe in waitingRecipeSO.recipeSOList)
        //         {
        //             foreach (KitchenObjectSO plateRecipe in plateKitchenObject.GetKitchenObjectList())
        //             {
        //                 if (recipe == plateRecipe)
        //                 {
        //                     hasSameRecipe = true;
        //                     break;
        //                 }
        //             }
        //         }
        //
        //         if (!hasSameRecipe)
        //         {
        //             matchRecipe = false;
        //             
        //         }
        //
        //         if (matchRecipe)
        //         {
        //             DeliverCorrectRecipeServerRpc(waitingRecipeSO.get);
        //
        //             return;
        //         }
        //     }
        //
        //     DeliverIncorrectRecipeServerRpc();
        // }

        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            bool matchRecipe = true;

            // has same recipe number
            if (waitingRecipeSOList[i].recipeSOList.Count == plateKitchenObject.GetKitchenObjectList().Count)
            {
                bool hasSameRecipe = false;

                // Compare whether there are the same recipe
                foreach (KitchenObjectSO recipe in waitingRecipeSOList[i].recipeSOList)
                {
                    foreach (KitchenObjectSO plateRecipe in plateKitchenObject.GetKitchenObjectList())
                    {
                        if (recipe == plateRecipe)
                        {
                            hasSameRecipe = true;
                            break;
                        }
                    }
                }

                if (!hasSameRecipe)
                {
                    matchRecipe = false;
                }

                if (matchRecipe)
                {
                    DeliverCorrectRecipeServerRpc(i);

                    return;
                }
            }

            DeliverIncorrectRecipeServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeSOIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSOIndex);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSOIndex)
    {
        deliveredCount++;
        waitingRecipeSOList.Remove(waitingRecipeSOList[waitingRecipeSOIndex]);

        deliveryDisableEvent?.Invoke(this, EventArgs.Empty);
        deliverySuccessEvent?.Invoke(this, EventArgs.Empty);
        deliverySuccessBoardEvent?.Invoke(this, EventArgs.Empty);
        deliveryAnimEvent?.Invoke(this, EventArgs.Empty);

        DeliveryStateUI.Instance.Show();

        spawnRecipeCount--;
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }

    [ClientRpc]
    private void DeliverIncorrectRecipeClientRpc()
    {
        deliveryFaildEvent?.Invoke(this, EventArgs.Empty);
        deliveryFailedBoardEvent?.Invoke(this, EventArgs.Empty);
        deliveryAnimEvent?.Invoke(this, EventArgs.Empty);

        DeliveryStateUI.Instance.Show();
    }

    public List<RecipeSO> GetWaitingRecipesList()
    {
        return waitingRecipeSOList;
    }

    public int GetDeliveredCount()
    {
        return deliveredCount;
    }
}