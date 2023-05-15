using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryStateAnimUI : MonoBehaviour
{
    private Animator animator;

    private const string DELIVERT_STATE = "DELIVERYSTATE";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.deliveryAnimEvent += OnDeliveryAnimEvent;
    }

    private void OnDeliveryAnimEvent(object sender, EventArgs e)
    {
        animator.SetTrigger(DELIVERT_STATE);
    }
}
