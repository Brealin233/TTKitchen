using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateComplateVisual : MonoBehaviour
{
    [Serializable]
    private struct KitchenObjectSOGameobject
    {
        public GameObject KitchenGameObject;
        public KitchenObjectSO KitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSOGameobject> kitchenObjectSOGameobjectList;
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    private void Start()
    {
        plateKitchenObject.complateVisualEvent += OnComplateVisualEvent;

        foreach (KitchenObjectSOGameobject kitchenObjectSOGameobject in kitchenObjectSOGameobjectList)
        {
            kitchenObjectSOGameobject.KitchenGameObject.SetActive(false);
        }
    }

    private void OnComplateVisualEvent(object sender, PlateKitchenObject.ComplateVisualEvent e)
    {
        foreach (KitchenObjectSOGameobject kitchenObjectSOGameobject in kitchenObjectSOGameobjectList)
        {
            if (kitchenObjectSOGameobject.KitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSOGameobject.KitchenGameObject.SetActive(true);
            }
        }
    }
}