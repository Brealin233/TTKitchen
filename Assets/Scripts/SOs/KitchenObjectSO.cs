using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KitchenData/KitchenObject")]
public class KitchenObjectSO : ScriptableObject
{
    public Sprite objSprite;
    public Transform prefabTransform;
    public string objName;
    public float cuttingTimeMax;

}
