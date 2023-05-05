using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler spawnPlateEvent;
    public event EventHandler removePlateEvent;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTime;
    private readonly float spawnPlateTimeMax = 4f;
    private int platesCount;
    private readonly int platesCountMax = 4;

    private void Update()
    {
        spawnPlateTime += Time.deltaTime;

        if (spawnPlateTime > spawnPlateTimeMax)
        {
            spawnPlateTime = 0f;

            if (platesCount < platesCountMax)
            {
                platesCount++;
                spawnPlateEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!playerController.HasKitchenObject() && HasKitchenObject())
        {
            if (platesCount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, playerController);
                removePlateEvent?.Invoke(this, EventArgs.Empty);
                platesCount--;
            }
        }
    }
}