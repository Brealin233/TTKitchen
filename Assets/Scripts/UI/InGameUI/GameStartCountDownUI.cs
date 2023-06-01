using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    public event EventHandler countDownEvent;

    public TextMeshProUGUI gameStartCountDownText;

    private float gameStartTimeMax = 3f;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (TTKitchenGameManager.Instance.IsGameCountDownState())
        {
            gameStartTimeMax -= Time.deltaTime;
            Show();

            gameStartCountDownText.text = Mathf.Ceil(Convert.ToSingle(gameStartTimeMax)).ToString();
            if (gameStartTimeMax < .1)
            {
                Hide();
            }
        }
    }

    private void Show()
    {
        gameStartCountDownText.gameObject.SetActive(true);
        countDownEvent?.Invoke(this, EventArgs.Empty);
    }

    private void Hide()
    {
        gameStartCountDownText.gameObject.SetActive(false);
    }
}