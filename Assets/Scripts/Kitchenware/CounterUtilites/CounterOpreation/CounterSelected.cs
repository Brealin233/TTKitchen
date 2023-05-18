using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CounterSelected : MonoBehaviour
{
    [SerializeField] private BaseCounter BaseCounterSelected;
    [SerializeField] private List<GameObject> visualGameObject;

    private void Start()
    {
        if (PlayerController.localInstance != null)
        {
            PlayerController.localInstance.BaseCounterSelectedEvent += InstanceOnBaseCounterSelectedEvent;
        }
        else
        {
            PlayerController.anyPlayerSpawnEvent += OnAnyPlayerSpawnEvent;
        }
    }

    private void OnAnyPlayerSpawnEvent(object sender, EventArgs e)
    {
        if (PlayerController.localInstance != null)
        {
            PlayerController.localInstance.BaseCounterSelectedEvent -= InstanceOnBaseCounterSelectedEvent;
            PlayerController.localInstance.BaseCounterSelectedEvent += InstanceOnBaseCounterSelectedEvent;
        }
    }

    private void InstanceOnBaseCounterSelectedEvent(object sender, PlayerController.BaseCounterSelectedEventArgs e)
    {
        foreach (GameObject gameObject in visualGameObject)
        {
            gameObject.SetActive(e.baseCounter == BaseCounterSelected);
        }
    }
}