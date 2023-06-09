using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkUITest : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        Show();
        
        startHostButton.onClick.AddListener(() =>
        {
            Debug.Log("HOST");
            MultipleKitchenObejctNetwork.Instance.StartHost();
            Hide();
        });
        
        startClientButton.onClick.AddListener(() =>
        {
            Debug.Log("CLIENT");
            MultipleKitchenObejctNetwork.Instance.StartClient();
            Hide();
        });
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
