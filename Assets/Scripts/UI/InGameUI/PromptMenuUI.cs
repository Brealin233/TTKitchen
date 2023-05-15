using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    
    private void Start()
    {
        Hide();
        
        TimeClockUI.promptTextEvent += OnPromptTextEvent;
    }

    private void OnPromptTextEvent(object sender, EventArgs e)
    {
        Show();

        promptText.text = "Menu & PAUSE(key) : " + GameInputManager.Instance.GetBindingsText(GameInputManager.Bindings.pause);
    }

    private void Show()
    {
        promptText.gameObject.SetActive(true);
    }

    private void Hide()
    {
        promptText.gameObject.SetActive(false);
    }
}
