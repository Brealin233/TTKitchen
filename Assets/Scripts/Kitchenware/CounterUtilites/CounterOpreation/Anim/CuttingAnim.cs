using System;
using UnityEngine;

public class CuttingAnim : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;

    private const string CuttingName = "Cut";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.counterVisualEvent += OnCuttingCounterAnimVisualEvents;
    }
    
    private void OnCuttingCounterAnimVisualEvents(object sender,IWasVisualCounter.counterVisualEventClass e)
    {
        animator.SetTrigger(CuttingName);
    }
}
