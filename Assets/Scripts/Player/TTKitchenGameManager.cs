using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TTKitchenGameManager : NetworkBehaviour
{
    public static TTKitchenGameManager Instance { get; private set; }

    public event EventHandler gameIntroduceEvent;
    public event EventHandler gameLocalPauseEvent;
    public event EventHandler gameLocalUnPauseEvent;
    public event EventHandler gameOverEvent;
    public event EventHandler gamePauseEvent;
    public event EventHandler gameUnPauseEvent;
    public event EventHandler changeGameStateEvent;
    public event EventHandler changeLocalPlayerReadyEvent;

    enum State
    {
        GameStart,
        GameStartCountDown,
        InGame,
        GameOut
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.GameStart);

    private NetworkVariable<float> gameStartCountDownTime = new NetworkVariable<float>();
    private float gameStartCountDownTimeMax = 3f;
    private NetworkVariable<float> inGameTime = new NetworkVariable<float>();
    private float inGameTimeMax = 300f;

    private bool isLocalGamePause;
    private NetworkVariable<bool> isGamePause = new NetworkVariable<bool>();
    private bool isGameStart = true;
    private bool isLocalPlayerReady;
    private bool clientGameDisconnected;

    private Dictionary<ulong, bool> playerReadyDic;
    private Dictionary<ulong, bool> playerPauseDic;

    private void Awake()
    {
        Instance = this;

        playerReadyDic = new Dictionary<ulong, bool>();
        playerPauseDic = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInputManager.Instance.inputInteractHandler += OnInputInteractHandler;

        gameIntroduceEvent?.Invoke(this, EventArgs.Empty);
        //SetGameStartState();

        gameStartCountDownTime.Value = gameStartCountDownTimeMax;
        inGameTime.Value = inGameTimeMax;
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += SatetOnValueChanged;
        isGamePause.OnValueChanged += IsGamePauseOnValueChanged;


        if (IsServer)
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    private void OnClientDisconnectCallback(ulong clientID)
    {
        clientGameDisconnected = true;
    }

    private void IsGamePauseOnValueChanged(bool previousvalue, bool newvalue)
    {
        if (isGamePause.Value)
        {
            Time.timeScale = 0f;

            gamePauseEvent?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;

            gameUnPauseEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SatetOnValueChanged(State previousvalue, State newvalue)
    {
        changeGameStateEvent?.Invoke(this, EventArgs.Empty);
    }

    private void LateUpdate()
    {
        if (!IsServer)
        {
            return;
        }

        switch (state.Value)
        {
            case State.GameStart:
                break;
            case State.GameStartCountDown:
                gameStartCountDownTime.Value -= Time.deltaTime;

                if (gameStartCountDownTime.Value < 0)
                {
                    state.Value = State.InGame;

                    // todo: can`t do anything
                }

                break;
            case State.InGame:
                inGameTime.Value -= Time.deltaTime;

                if (inGameTime.Value < 0)
                {
                    state.Value = State.GameOut;
                }

                break;
            case State.GameOut:
                gameOverEvent?.Invoke(this, EventArgs.Empty);
                break;
        }

        if (clientGameDisconnected)
        {
            clientGameDisconnected = false;
            SetGamePauseState();
        }
    }


    private void OnInputInteractHandler(object sender, EventArgs e)
    {
        if (state.Value == State.GameStart)
        {
            isLocalPlayerReady = true;
            changeLocalPlayerReadyEvent?.Invoke(this, EventArgs.Empty);

            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDic[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientReady = true;

        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDic.ContainsKey(clientID) || !playerReadyDic[clientID])
            {
                allClientReady = false;
                break;
            }
        }

        if (allClientReady)
        {
            //SetGameStartState();
            state.Value = State.GameStartCountDown;
        }
    }

    public bool IsInGameState()
    {
        return state.Value == State.InGame;
    }

    public bool IsGameOutState()
    {
        return state.Value == State.GameOut;
    }

    public bool IsGameCountDownState()
    {
        return state.Value == State.GameStartCountDown;
    }

    public bool isGameStartState()
    {
        return state.Value == State.GameStart;
    }

    public float GetInGameTime()
    {
        return inGameTime.Value;
    }

    public float GetInGameTimeMax()
    {
        return inGameTimeMax;
    }

    public bool GetLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }

    public void SetGamePauseState()
    {
        isLocalGamePause = !isLocalGamePause;

        if (isLocalGamePause)
        {
            SetPauseServerRpc();

            gameLocalPauseEvent?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            SetUnPauseServerRpc();

            gameLocalUnPauseEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    // private void SetGameStartState()
    // {
    //     isGameStart = !isGameStart;
    //
    //     Time.timeScale = isGameStart ? 0f : 1f;
    // }

    [ServerRpc(RequireOwnership = false)]
    private void SetPauseServerRpc(ServerRpcParams rpcParams = default)
    {
        playerPauseDic[rpcParams.Receive.SenderClientId] = true;

        GetPlayerPause();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetUnPauseServerRpc(ServerRpcParams rpcParams = default)
    {
        playerPauseDic[rpcParams.Receive.SenderClientId] = false;

        GetPlayerPause();
    }

    private void GetPlayerPause()
    {
        foreach (var clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPauseDic.ContainsKey(clientID) && playerPauseDic[clientID])
            {
                isGamePause.Value = true;
                return;
            }
        }

        isGamePause.Value = false;
    }
}