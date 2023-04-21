using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounterSelected : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;

    private void Start()
    {
        PlayerController.Instance.ClearCounterSelectedEvent += InstanceOnClearCounterSelectedEvent;
    }

    private void InstanceOnClearCounterSelectedEvent(object sender, PlayerController.ClearCounterSelectedEventArgs e)
    {
        if (e.clearCounter == clearCounter)
        {
            Show();
        }
        else Hide();
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        visualGameObject.SetActive(false);
    }
}
