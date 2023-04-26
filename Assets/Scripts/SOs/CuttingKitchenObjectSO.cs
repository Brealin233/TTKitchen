using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "KitchenData/SliceKitchenObject")]
public class CuttingKitchenObjectSO : ScriptableObject
{
    public KitchenObject inputKitchenObject;
    [FormerlySerializedAs("outputKKitchenObject")] public KitchenObject outputKitchenObject;
}
