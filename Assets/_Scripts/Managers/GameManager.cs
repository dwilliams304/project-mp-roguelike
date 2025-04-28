using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum GameStateType
{
    Waiting,
    Countdown,
    Active,
    RoundEnd,
    GameOver
}
namespace ContradictiveGames.Managers
{
    public class GameManager : NetworkBehaviour
    {
        //Instance
        public static GameManager Instance;

        //Events
        public event Action<GameStateType> GameStateChanged;


        [Header("Game Settings")]
        [Tooltip("How long we want the initial countdown to be")]
        [SerializeField] private float countdownTimer;
        [Tooltip("How long in seconds the game can go on")]
        [SerializeField] private float maxGameTime;


        [Header("GAME SETTINGS")]
        public GameSettings gameSettings;

        //Game State related
        public NetworkVariable<GameStateType> CurrentGameStateType = new NetworkVariable<GameStateType>(GameStateType.Waiting);
        public NetworkVariable<float> CurrentCountdownTimer = new NetworkVariable<float>(); //Current time for countdown
        public NetworkVariable<float> MaxGameTime = new NetworkVariable<float>();

        //Active timers
        public NetworkVariable<float> CurrentActiveGameTimer = new NetworkVariable<float>(0f); //Current time for active game

        //States
        private GameState CurrentGameState;
        public WaitingState waitingState;
        public CountdownState countdownState;
        public ActiveState activeState;
        public RoundEndState roundEndState;
        public GameOverState gameOverState;

        //Private variables
        private Dictionary<ulong, bool> playersReadyDictionary;


        private void Awake()
        {
            Instance = this;
        }


        public override void OnNetworkSpawn()
        {
            waitingState = new WaitingState();
            countdownState = new CountdownState();
            activeState = new ActiveState();
            roundEndState = new RoundEndState();
            gameOverState = new GameOverState();

            playersReadyDictionary = new Dictionary<ulong, bool>();

            CurrentGameStateType.OnValueChanged += OnGameStateChanged;

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                InitializeGameSettings();
            }

        }

        private void OnClientConnected(ulong clientId)
        {
            if (!playersReadyDictionary.ContainsKey(clientId))
            {
                playersReadyDictionary[clientId] = false;
            }
        }

        private void InitializeGameSettings()
        {
            CurrentGameStateType.Value = GameStateType.Waiting;
            CurrentCountdownTimer.Value = gameSettings.CountdownTimer; //Current time for countdown
            MaxGameTime.Value = gameSettings.MaxGameTime;

            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                playersReadyDictionary[client.ClientId] = false;
            }

            ChangeGameState(waitingState, GameStateType.Waiting);
        }


        private void OnGameStateChanged(GameStateType prevState, GameStateType newState)
        {
            GameStateChanged?.Invoke(newState);
        }

        public override void OnDestroy()
        {
            if (NetworkManager.Singleton != null) NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            CurrentGameStateType.OnValueChanged -= OnGameStateChanged;
        }


        private void Update()
        {
            if (!IsServer) return;

            if (CurrentGameState != null) CurrentGameState.StateUpdate(this);
        }


        public void ChangeGameState(GameState newState, GameStateType newGameStateType)
        {
            if (CurrentGameState != null)
            {
                CurrentGameState.StateExit(this);
            }

            CurrentGameState = newState;
            CurrentGameStateType.Value = newGameStateType;
            CurrentGameState.StateEnter(this);

        }
        private GameState GetGameStateFromType(GameStateType type)
        {
            return type switch
            {
                GameStateType.Waiting => waitingState,
                GameStateType.Countdown => countdownState,
                GameStateType.Active => activeState,
                GameStateType.RoundEnd => roundEndState,
                GameStateType.GameOver => gameOverState,
                _ => waitingState
            };
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayerReadyServerRpc(bool isReady, ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            if (!playersReadyDictionary.ContainsKey(clientId))
            {
                playersReadyDictionary.Add(clientId, isReady);
            }
            else playersReadyDictionary[clientId] = isReady;

            CheckAllPlayersReady();
        }
        private void CheckAllPlayersReady()
        {
            bool allAreReady = true;

            foreach (bool clientReady in playersReadyDictionary.Values)
            {
                if (!clientReady) allAreReady = false;
            }

            if (allAreReady)
            {
                ChangeGameState(countdownState, GameStateType.Countdown);
            }
        }
    }
}