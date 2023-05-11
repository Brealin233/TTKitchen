using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryRecipeSingleUIManager : MonoBehaviour
{
    public TextMeshProUGUI recipeName;
    public Transform iconContainer;
    public Image icon;

    public void SetRecipeIcon(RecipeSO recipeSO)
    {
        iconContainer.gameObject.SetActive(true);
        recipeName.text = recipeSO.recipeName;

        foreach (Transform child in iconContainer) Destroy(child);
        
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.recipeSOList)
        {
            Image iconImage = Instantiate(icon, iconContainer);
            iconImage.sprite = kitchenObjectSO.objSprite;
        }
    }
}
