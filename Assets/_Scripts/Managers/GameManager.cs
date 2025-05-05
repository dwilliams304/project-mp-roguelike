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




        // public override void OnStartClient()
        // {
        //     gameStateMachine = new GameStateMachine();
        //     gameStateMachine.InitializeStateMachine();


        // }


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