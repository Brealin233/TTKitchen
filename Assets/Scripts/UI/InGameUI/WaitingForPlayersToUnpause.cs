using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayersToUnpause : MonoBehaviour
{
    private void Start()
    {
        TTKitchenGameManager.Instance.gamePauseEvent += OnGamePauseEvent;
        TTKitchenGameManager.Instance.gameUnPauseEvent += OnGameUnPauseEvent;
        
        Hide();
    }

    private void OnGameUnPauseEvent(object sender, EventArgs e)
    {
        Hide();
    }

    private void OnGamePauseEvent(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
