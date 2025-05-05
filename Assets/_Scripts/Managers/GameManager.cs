using System;
using System.Collections.Generic;
using UnityEngine;
using ContradictiveGames.State;
using FishNet.Object;
using FishNet.Object.Synchronizing;

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
        public readonly SyncVar<GameStateType> CurrentGameStateType;
        public readonly SyncVar<float> CurrentCountdownTimer; //Current time for countdown
        public readonly SyncVar<float> MaxGameTime;

        //Active timers
        public readonly SyncVar<float> CurrentActiveGameTimer; //Current time for active game

        //States
        private GameStateMachine gameStateMachine;



        private void Awake() => Instance = this;

        // public override void OnNetworkSpawn()
        // {
        //     gameStateMachine = new GameStateMachine();
        //     gameStateMachine.InitializeStateMachine();


        //     CurrentGameStateType.OnChange += OnGameStateChanged;

        //     if (IsServer)
        //     {
        //         InitializeGameSettings();
        //     }

        // }



        private void InitializeGameSettings()
        {
            // CurrentCountdownTimer.Value = gameSettings.CountdownTimer; //Current time for countdown
            // MaxGameTime.Value = gameSettings.MaxGameTime;

            
        }


        public void UpdateState(StateNode<GameStateMachine> stateNode){
            if(!IsServer) return;
            // CurrentGameStateType.Value = GetStateType(stateNode);
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
        private void OnGameStateChanged(GameStateType prevState, GameStateType newState, bool asServer) => GameStateChanged?.Invoke(newState);


        // public override void OnDestroy()
        // {
        //     CurrentGameStateType.OnChange -= OnGameStateChanged;
        // }


        private void Update()
        {
            if (!IsServer) return;

            if (gameStateMachine.currentState != null) gameStateMachine.StateUpdate();
        }



        public float GetXPScaler(int level) => gameSettings.XPScaler.Evaluate(level);
    }
}