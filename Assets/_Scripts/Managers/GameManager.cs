using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using ContradictiveGames.State;

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


        [Header("GAME SETTINGS")]
        public GameSettings gameSettings;

        //Game State related
        public NetworkVariable<GameStateType> CurrentGameStateType = new NetworkVariable<GameStateType>(GameStateType.Waiting);
        public NetworkVariable<float> CurrentCountdownTimer = new NetworkVariable<float>(); //Current time for countdown
        public NetworkVariable<float> MaxGameTime = new NetworkVariable<float>();

        //Active timers
        public NetworkVariable<float> CurrentActiveGameTimer = new NetworkVariable<float>(0f); //Current time for active game

        //States
        private GameStateMachine gameStateMachine;

        //Private variables
        private Dictionary<ulong, bool> playersReadyDictionary;


        private void Awake() => Instance = this;

        public override void OnNetworkSpawn()
        {
            gameStateMachine = new GameStateMachine();
            gameStateMachine.InitializeStateMachine();

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
            CurrentCountdownTimer.Value = gameSettings.CountdownTimer; //Current time for countdown
            MaxGameTime.Value = gameSettings.MaxGameTime;

            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                playersReadyDictionary[client.ClientId] = false;
            }
            
        }


        public void UpdateState(StateNode<GameStateMachine> stateNode){
            if(!IsServer) return;
            CurrentGameStateType.Value = GetStateType(stateNode);
        }

        private GameStateType GetStateType(StateNode<GameStateMachine> state)
        {
            return state switch
            {
                WaitingState => GameStateType.Waiting,
                CountdownState => GameStateType.Countdown,
                ActiveState => GameStateType.Active,
                RoundEndState => GameStateType.RoundEnd,
                GameOverState => GameStateType.GameOver,
                _ => throw new System.ArgumentException("Unknown state passed to GetStateType")
            };
        }
        private void OnGameStateChanged(GameStateType prevState, GameStateType newState) => GameStateChanged?.Invoke(newState);


        public override void OnDestroy()
        {
            if (NetworkManager.Singleton != null) NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            CurrentGameStateType.OnValueChanged -= OnGameStateChanged;
        }


        private void Update()
        {
            if (!IsServer) return;

            if (gameStateMachine.currentState != null) gameStateMachine.StateUpdate();
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
                if(gameSettings.skipCountdownTimer){
                    gameStateMachine.ChangeState(gameStateMachine.ActiveState);
                }
                else{
                    gameStateMachine.ChangeState(gameStateMachine.CountdownState);
                }
            }
        }


        public float GetXPScaler(int level) => gameSettings.XPScaler.Evaluate(level);
    }
}