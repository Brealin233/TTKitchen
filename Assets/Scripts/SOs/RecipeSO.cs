using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KitchenData/RecipeSO")]
public class RecipeSO:ScriptableObject
{
    public List<KitchenObjectSO> recipeSOList;
    public string recipeName;
}
