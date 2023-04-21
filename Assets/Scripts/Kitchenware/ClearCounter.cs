using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO initPrefab;
    [SerializeField] private Transform counterTopPoint;

    public void InteractPlayer()
    {
        Debug.Log("hello tt");
        Transform prefab = Instantiate(initPrefab.prefabTransform, counterTopPoint);
        prefab.localPosition = Vector3.zero;

        Debug.Log(prefab.GetComponent<KitchenObject>().GetKitchenObjectSO().objName);
    }
}
