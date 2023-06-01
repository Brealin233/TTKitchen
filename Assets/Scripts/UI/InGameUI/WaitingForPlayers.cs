using System;
using UnityEngine;

public class WaitingForPlayers : MonoBehaviour
{
    private void Start()
    {
        Hide();
        
        TTKitchenGameManager.Instance.changeGameStateEvent += OnChangeGameStateEvent;
        TTKitchenGameManager.Instance.changeLocalPlayerReadyEvent += OnChangeLocalPlayerReadyEvent;
    }

    private void OnChangeGameStateEvent(object sender, EventArgs e)
    {
        Hide();
    }

    private void OnChangeLocalPlayerReadyEvent(object sender, EventArgs e)
    {
        if (TTKitchenGameManager.Instance.GetLocalPlayerReady())
        {
            Show();
        }
    }

    private void Show()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    
    private void Hide()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
