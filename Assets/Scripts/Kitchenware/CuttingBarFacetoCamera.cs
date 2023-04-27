using System;
using Unity.VisualScripting;
using UnityEngine;


public class CuttingBarFacetoCamera : MonoBehaviour
{
    private enum FaceToCuttingProgressBar
    {
        LookAt,
        LookAtInverted,
        Forward,
        ForwardInverted
    }

    private CounterVisual counterVisual;
    
    [SerializeField] private FaceToCuttingProgressBar faceToCuttingProgressBar;

    private void Awake()
    {
        counterVisual = GetComponent<CounterVisual>();
    }

    private void Start()
    {
        counterVisual.FaceToCameraEvent += OnFaceToCameraEvent;
    }

    private void OnFaceToCameraEvent(object sender, EventArgs e)
    {
        switch (faceToCuttingProgressBar)
        {
            case FaceToCuttingProgressBar.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case FaceToCuttingProgressBar.LookAtInverted:
                Vector3 dirFromCamera = transform.transform.position - Camera.main.transform.position; 
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case FaceToCuttingProgressBar.Forward:
                transform.forward = Camera.main.transform.forward;
                break;
            case FaceToCuttingProgressBar.ForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}