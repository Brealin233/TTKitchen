using System;
using UnityEngine;
using UnityEngine.UI;

public class CounterVisual : MonoBehaviour
{
    public event EventHandler FaceToCameraEvent;

    [SerializeField] private GameObject iWasVisualObject;
    [SerializeField] private GameObject counterVisualObject;
    [SerializeField] private Image fillImage;

    private IWasVisualCounter iWasVisualCounter;

    private void Start()
    {
        if (iWasVisualObject != null)
        {
            iWasVisualCounter = iWasVisualObject.GetComponent<IWasVisualCounter>();
            iWasVisualCounter.counterVisualEvent += OnCounterVisualEvent;
        }
    }

    private void LateUpdate()
    {
        FaceToCameraEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnCounterVisualEvent(object sender, IWasVisualCounter.counterVisualEventClass e)
    {
        fillImage.fillAmount = e.fillAmount;
        if (e.fillAmount != 0 && e.fillAmount != 1)
            Show();
        else
            Hide();
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