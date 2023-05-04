using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "KitchenData/FryingKitchenObject")]
public class FryingObjectSO : ScriptableObject
{
    public KitchenObject inputKitchenObject;
    public KitchenObject outputKitchenObject;
    public float fryingTimerMax;
}
