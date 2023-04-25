using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerAnim : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private const string containerAnimTriggerName = "OpenClose";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.handleGrabbedObject += ContainerCounterOnhandleGrabbedObject;
    }

    private void ContainerCounterOnhandleGrabbedObject(object sender, EventArgs e)
    {
        animator.SetTrigger(containerAnimTriggerName);
    }
}