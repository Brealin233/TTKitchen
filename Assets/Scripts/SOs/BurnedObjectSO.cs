using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "KitchenData/BurnedObject")]
public class BurnedObjectSO : ScriptableObject
{
    public KitchenObject inputKitchenObject;
    public KitchenObject outputKitchenObject;
    public float burnedTimerMax;
}
