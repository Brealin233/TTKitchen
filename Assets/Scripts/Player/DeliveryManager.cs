using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

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
                    Debug.Log("Correct Recipe Answer!");
                    waitingRecipeSOList.Remove(waitingRecipeSO);
                    spawnRecipeCount--;

                    return;
                }
            }

            Debug.Log("NO Match Found");
        }
    }
}