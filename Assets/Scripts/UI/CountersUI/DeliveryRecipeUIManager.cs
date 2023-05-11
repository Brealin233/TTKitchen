using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryRecipeUIManager : MonoBehaviour
{
    [SerializeField] private GameObject deliveryRecipeTemplate;

    private void Start()
    {
        foreach (Transform recipeTemplate in transform)
        {
            if (recipeTemplate == deliveryRecipeTemplate) continue;
            Destroy(recipeTemplate.gameObject);
        }

        DeliveryManager.Instance.deliverySpawnEvent += OnDeliverySpawnEvent;
        DeliveryManager.Instance.deliveryDisableEvent += OnDeliveryDisableEvent;
    }

    private void OnDeliveryDisableEvent(object sender, EventArgs e)
    {
        UpdateRecipeUI();
    }

    private void OnDeliverySpawnEvent(object sender, EventArgs e)
    {
        UpdateRecipeUI();
    }

    private void UpdateRecipeUI()
    {
        deliveryRecipeTemplate.SetActive(true);

        foreach (Transform child in transform) Destroy(child.gameObject);

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipesList())
        {
            var recipe = Instantiate(deliveryRecipeTemplate, transform).GetComponent<DeliveryRecipeSingleUIManager>();
            recipe.SetRecipeIcon(recipeSO);
        }
    }
}