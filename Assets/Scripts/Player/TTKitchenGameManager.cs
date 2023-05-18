using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TTKitchenGameManager : MonoBehaviour
{
    public static TTKitchenGameManager Instance { get; private set; }

    
    public event EventHandler gameIntroduceEvent;
    public event EventHandler gamePauseEvent;
    public event EventHandler gameUnPauseEvent;
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
    private float inGameTimeMax = 300f;
    private bool isGamePause;
    private bool isGameStart;

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
                SetGameStartState();
                gameIntroduceEvent?.Invoke(this,EventArgs.Empty);
                break;
            case State.GameStartCountDown:
                gameStartCountDownTime -= Time.deltaTime;
                
                if (gameStartCountDownTime < 0)
                {
                    state = State.InGame;
                    
                    // todo: can`t do anything
                }
                break;
            case State.InGame:
                inGameTime -= Time.deltaTime;
                
                if (inGameTime < 0)
                {
                    state = State.GameOut;
                }
                break;
            case State.GameOut:
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

    public void SetGameStartCountDownState()
    {
        state = State.GameStartCountDown;
    }

    public void SetGamePauseState()
    {
        isGamePause = !isGamePause;

        if (isGamePause)
        {
            Time.timeScale = 0f;
            
            gamePauseEvent?.Invoke(this,EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            
            gameUnPauseEvent?.Invoke(this,EventArgs.Empty);
        }
    }
    
    public void SetGameStartState()
    {
        isGameStart = !isGameStart;

        if (isGameStart)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public bool GetGameStart()
    {
        return isGameStart;
    }
}
