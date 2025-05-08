using UnityEngine;
using ContradictiveGames.State;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.CodeGenerating;
using FishNet.Connection;

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



        [Header("GAME SETTINGS")]
        public GameSettings gameSettings;

        //Game State related
        [AllowMutableSyncType] public SyncVar<GameStateType> CurrentGameStateType;
        [AllowMutableSyncType] public SyncVar<float> CurrentCountdownTimer; //Current time for countdown
        [AllowMutableSyncType] public SyncVar<float> MaxGameTime;
        // [AllowMutableSyncType] public SyncDictionary<NetworkConnection, bool> ReadyPlayers;

        //Active timers
        [AllowMutableSyncType] public SyncVar<float> CurrentActiveGameTimer; //Current time for active game

        //States
        private GameStateMachine gameStateMachine;

        private void Awake(){
            Instance = this;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            gameStateMachine = new GameStateMachine();
            gameStateMachine.InitializeStateMachine();

            CurrentGameStateType.Value = GameStateType.Waiting;
            CurrentCountdownTimer.Value = gameSettings.CountdownTimer;
            MaxGameTime.Value = gameSettings.MaxGameTime;
            CurrentActiveGameTimer.Value = gameSettings.MaxGameTime;
        }



        public void UpdateState(StateNode<GameStateMachine> state){
            CurrentGameStateType.Value = GetStateType(state);
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

        
    }
}