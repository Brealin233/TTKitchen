using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    
    private const string IS_WALKING = "IsWalking";

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING,playerController.IsWalking());
    }
}
