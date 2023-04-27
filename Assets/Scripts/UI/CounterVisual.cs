using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CounterVisual : MonoBehaviour
{
    public event EventHandler FaceToCameraEvent; 

    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private GameObject counterVisualObject;
    [SerializeField] private Image fillImage;

    private void Start()
    {
        cuttingCounter.counterVisualEvent += OnCounterVisualEvent;
    }

    private void LateUpdate()
    {
        FaceToCameraEvent?.Invoke(this,EventArgs.Empty);
    }

    private void OnCounterVisualEvent(object sender, CuttingCounter.counterVisualEventClass e)
    {
        if (e.fillAmount - 1 != cuttingCounter.kitchenObject.GetKitchenObjectSO().cuttingTimeMax)
        {
            fillImage.fillAmount =
                (e.fillAmount - 1) / cuttingCounter.kitchenObject.GetKitchenObjectSO().cuttingTimeMax;
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        counterVisualObject.SetActive(true);
    }

    private void Hide()
    {
        counterVisualObject.SetActive(false);
        fillImage.fillAmount = 0;
    }
}