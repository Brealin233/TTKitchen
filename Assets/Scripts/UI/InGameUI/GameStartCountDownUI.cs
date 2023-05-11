using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    public TextMeshProUGUI gameStartCountDownText;
    
    private float gameStartTimeMax = 3f;
    
    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        gameStartTimeMax -= Time.deltaTime;
        Show();
        
        gameStartCountDownText.text = Mathf.Ceil(Convert.ToSingle(gameStartTimeMax)).ToString();
        if (gameStartTimeMax < 0)
        {
            Hide();
        }
    }

    private void Show()
    {
        gameStartCountDownText.gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameStartCountDownText.gameObject.SetActive(false);
    }
}
