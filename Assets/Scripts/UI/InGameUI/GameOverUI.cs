using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private List<Transform> gameOverUIList;
    [SerializeField] private Transform deliveredCountTransform;

    private void Start()
    {
        TTKitchenGameManager.Instance.gameOverEvent += OnGameOverEvent;
        
        Hide();
    }

    private void OnGameOverEvent(object sender, EventArgs e)
    {
        if (TTKitchenGameManager.Instance.IsGameOutState())
        {
            Show();

            deliveredCountTransform.GetComponent<TextMeshProUGUI>().text =
                DeliveryManager.Instance.GetDeliveredCount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (var child in gameOverUIList)
        {
            child.gameObject.SetActive(true);
        }
        deliveredCountTransform.gameObject.SetActive(true);
    }

    private void Hide()
    {
        foreach (var child in gameOverUIList)
        {
            child.gameObject.SetActive(false);
        }
        deliveredCountTransform.gameObject.SetActive(false);
    }
}
