using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlateComplateVisualUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform templateIcon;

    private void Start()
    {
        plateKitchenObject.complateVisualEvent += OnComplateVisualEvent;
    }

    private void OnComplateVisualEvent(object sender, PlateKitchenObject.ComplateVisualEvent e)
    {
        UpdateUIElement();
    }

    private void UpdateUIElement()
    {
        foreach (Transform child in transform)
        {
            if (child == templateIcon) continue;
            
            Destroy(child.gameObject);
        }
        
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectList())
        {
            var spriteTransform = Instantiate(templateIcon, transform).GetComponent<PlateComplateVisualIcon>();
            spriteTransform.SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
