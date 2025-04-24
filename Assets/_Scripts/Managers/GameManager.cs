using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum GameStateType
{
    Spawning,
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
        public static GameManager Instance;

        [Header("Game Settings")]
        [Tooltip("How long we want the initial countdown to be")]
        [SerializeField] private float countdownTimer;
        [Tooltip("How long in seconds the game can go on")]
        [SerializeField] private float maxGameTime;


        [Header("DEVELOPMENT SETTINGS")]
        public DeveloperSettings developerSettings;

        public event Action<GameState> GameStateChanged;
        public event Action OnLocalPlayerReady;


        public NetworkVariable<float> currentCountdownTimer = new NetworkVariable<float>(10f); //Current time for countdown
        public NetworkVariable<float> currentActiveGameTimer = new NetworkVariable<float>(0f); //Current time for active game
        public float MaxGameTime = 6000f;

        public NetworkVariable<GameStateType> currentGameStateType = new NetworkVariable<GameStateType>(GameStateType.Spawning);

        private GameState currentGameState;
        public SpawningState spawningState;
        public WaitingState waitingState;
        public CountdownState countdownState;
        public ActiveState activeState;
        public RoundEndState roundEndState;
        public GameOverState gameOverState;


        private bool localPlayerReady;
        private Dictionary<ulong, bool> playersReadyDictionary;

        private void Awake()
        {
            Instance = this;

        }


        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                spawningState = new SpawningState();
                waitingState = new WaitingState();
                countdownState = new CountdownState();
                activeState = new ActiveState();
                roundEndState = new RoundEndState();
                gameOverState = new GameOverState();

                ChangeGameState(spawningState, GameStateType.Spawning);
            }
            playersReadyDictionary = new Dictionary<ulong, bool>();
            currentGameStateType.OnValueChanged += OnGameStateChanged;


        }

        private void OnGameStateChanged(GameStateType prevState, GameStateType newState)
        {
            GameStateChanged?.Invoke(GetGameStateFromType(newState));
        }

        public override void OnDestroy()
        {
            currentGameStateType.OnValueChanged -= OnGameStateChanged;
        }


        private void Update()
        {
            if (!IsServer) return;

            if (currentGameState != null) currentGameState.StateUpdate(this);
        }



        public void ChangeGameState(GameState newState, GameStateType newGameStateType)
        {
            if (currentGameState != null)
            {
                currentGameState.StateExit(this);
            }

            currentGameState = newState;
            currentGameStateType.Value = newGameStateType;
            currentGameState.StateEnter(this);

        }

        public bool IsLocalPlayerReady() => localPlayerReady;

        public void NotifyServerPlayerIsReady()
        {
            NotifyServerPlayerIsReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void NotifyServerPlayerIsReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            if (currentGameStateType.Value != GameStateType.Waiting)
            {
                Debug.LogWarning("Player ready called in wrong state.");
                return;
            }
            localPlayerReady = true;
            OnLocalPlayerReady?.Invoke();

            playersReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

            bool allClientsReady = true;
            foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playersReadyDictionary.ContainsKey(id) || !playersReadyDictionary[id])
                {
                    allClientsReady = false;
                    break;
                }
            }
            if (allClientsReady)
            {
                ChangeGameState(countdownState, GameStateType.Countdown);
            }

        }


        private GameState GetGameStateFromType(GameStateType type)
        {
            return type switch
            {
                GameStateType.Spawning => spawningState,
                GameStateType.Waiting => waitingState,
                GameStateType.Countdown => countdownState,
                GameStateType.Active => activeState,
                GameStateType.RoundEnd => roundEndState,
                GameStateType.GameOver => gameOverState,
                _ => waitingState
            };
        }
    }
}