using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectedUI : MonoBehaviour
{
    [SerializeField] private Button reGameButton;
    [SerializeField] private Button quitGameButton;

    private void Start()
    {
        reGameButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadScene(Loader.Scene.MainMenuScene);
        });
        
        quitGameButton.onClick.AddListener(Application.Quit);
        
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        
        Hide();
    }

    private void OnClientDisconnectCallback(ulong clientID)
    {
        if (clientID == NetworkManager.ServerClientId)
        {
            Show();
        }
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
