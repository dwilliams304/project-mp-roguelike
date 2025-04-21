using UnityEngine;


public enum GameState
{
    Waiting,
    Countdown,
    Active,
    Ended
}


namespace ContradictiveGames.Managers
{

    public class GameManager : PersistentSingleton<GameManager>
    {
        [Header("Game Settings")]
        [Tooltip("How long we want the initial countdown to be")] 
        [SerializeField] private float countdownTimer;
        [Tooltip("How long in seconds the game can go on")] 
        [SerializeField] private float maxGameTime;


        [Header("DEVELOPMENT SETTINGS")]
        public DeveloperSettings developerSettings;

        public GameState CurrentState { get; private set; }

        private float currentCountdownTimer; //Current time for countdown
        private float currentActiveGameTimer; //Current time for active game


    }
}