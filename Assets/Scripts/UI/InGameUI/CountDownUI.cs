using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] private GameStartCountDownUI gameStartCountDownUI;
    private Animator animator;

    private const string COUNT_DOWN = "COUNTDOWN";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameStartCountDownUI.countDownEvent += OnCountDownEvent;
    }

    private void OnCountDownEvent(object sender, EventArgs e)
    {
        animator.SetTrigger(COUNT_DOWN);
    }
}
