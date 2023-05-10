using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TTKitchenGameManager : MonoBehaviour
{
    public static TTKitchenGameManager Instance { get; private set; }

    public event EventHandler gameOverEvent;
    enum State
    {
        GameStart,
        GameStartCountDown,
        InGame,
        GameOut
    }

    private State state;

    private float gameStartCountDownTime;
    private float gameStartCountDownTimeMax = 3f;
    private float inGameTime;
    private float inGameTimeMax = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        state = State.GameStart;
        
        gameStartCountDownTime = gameStartCountDownTimeMax;
        inGameTime = inGameTimeMax;
    }

    private void Update()
    {
        switch (state)
        {
            case State.GameStart:
                state = State.GameStartCountDown;
                Debug.Log(state);

                break;
            case State.GameStartCountDown:
                gameStartCountDownTime -= Time.deltaTime;
                
                Debug.Log(state);

                if (gameStartCountDownTime < 0)
                {
                    state = State.InGame;

                }
                break;
            case State.InGame:
                inGameTime -= Time.deltaTime;
                
                Debug.Log(state);

                if (inGameTime < 0)
                {
                    state = State.GameOut;
                }
                break;
            case State.GameOut:
                Debug.Log(state);
                gameOverEvent?.Invoke(this,EventArgs.Empty);
                break;
        }
    }

    public bool IseInGameState()
    {
        return state == State.InGame;
    }

    public bool IsGameOutState()
    {
        return state == State.GameOut;
    }

    public float GetInGameTime()
    {
        return inGameTime;
    }
    
    public float GetInGameTimeMax()
    {
        return inGameTimeMax;
    }
}
