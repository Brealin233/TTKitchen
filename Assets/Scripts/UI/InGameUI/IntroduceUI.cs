using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroduceUI : MonoBehaviour
{
    public static IntroduceUI Instance { get; private set; }
    
    [SerializeField] private Image introduceImage;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI InteractText;
    [SerializeField] private TextMeshProUGUI InteractAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Show();
        
        TTKitchenGameManager.Instance.gameIntroduceEvent += OnGameIntroduceEvent;
        TTKitchenGameManager.Instance.changeLocalPlayerReadyEvent += OnChangeLocalPlayerReadyEvent;

        moveUpText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Move_UP);
        moveDownText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Move_Down);
        moveLeftText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Move_Left);
        moveRightText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Move_Right);
        InteractText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.Interact);
        InteractAlternateText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.InteractAlternate);
        pauseText.text = GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.pause);
    }

    private void OnChangeLocalPlayerReadyEvent(object sender, EventArgs e)
    {
        if (TTKitchenGameManager.Instance.GetLocalPlayerReady())
        {
            Hide();
        }
    }

    private void OnGameIntroduceEvent(object sender, EventArgs e)
    {
        Show();
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
