using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler handleGrabbedObject;
    [SerializeField] private KitchenObjectSO initPrefab;

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            Transform prefab = Instantiate(initPrefab.prefabTransform);
            handleGrabbedObject?.Invoke(this,EventArgs.Empty);
            prefab.GetComponent<KitchenObject>().SetKitchenObjectParent(playerController);
        }
    }
}
