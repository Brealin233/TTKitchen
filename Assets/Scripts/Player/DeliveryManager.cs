using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler deliverySpawnEvent;
    public event EventHandler deliveryDisableEvent;
    public event EventHandler deliverySuccessEvent;
    public event EventHandler deliveryFaildEvent;
    
    
    [SerializeField] private RecipeListSO recipeList;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTime;
    private readonly float spawnRecipeTimeMax = 4f;
    private int spawnRecipeCount;
    private readonly int spawnRecipeCountMax = 4;

    private void Awake()
    {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTime -= Time.deltaTime;

        if (spawnRecipeTime < 0f)
        {
            spawnRecipeTime = spawnRecipeTimeMax;

            if (spawnRecipeCount < spawnRecipeCountMax)
            {
                int randomRecipe = Random.Range(0, recipeList.RecipeListSOList.Count);

                spawnRecipeCount++;
                
                waitingRecipeSOList.Add(recipeList.RecipeListSOList[randomRecipe]);
                
                deliverySpawnEvent?.Invoke(this,EventArgs.Empty);
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach (RecipeSO waitingRecipeSO in waitingRecipeSOList)
        {
            bool matchRecipe = true;

            // has same recipe number
            if (waitingRecipeSO.recipeSOList.Count == plateKitchenObject.GetKitchenObjectList().Count)
            {
                bool hasSameRecipe = false;

                // Compare whether there are the same recipe
                foreach (KitchenObjectSO recipe in waitingRecipeSO.recipeSOList)
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
                    waitingRecipeSOList.Remove(waitingRecipeSO);
                    
                    deliveryDisableEvent?.Invoke(this,EventArgs.Empty);
                    deliverySuccessEvent?.Invoke(this,EventArgs.Empty);

                    spawnRecipeCount--;

                    return;
                }
            }

            deliveryFaildEvent?.Invoke(this,EventArgs.Empty);
        }
    }

    public List<RecipeSO> GetWaitingRecipesList()
    {
        return waitingRecipeSOList;
    }
}