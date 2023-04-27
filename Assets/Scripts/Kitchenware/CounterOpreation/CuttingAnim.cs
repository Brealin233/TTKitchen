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
        cuttingCounter.counterVisualEvent += OnCuttingCounterAnimVisualEvent;
    }

    private void OnCuttingCounterAnimVisualEvent(object sender, CuttingCounter.counterVisualEventClass e)
    {
        animator.SetTrigger(CuttingName);
    }
}
