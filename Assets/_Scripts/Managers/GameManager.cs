using System;
using UnityEngine;

namespace ContradictiveGames.Managers
{
    [System.Flags]
    public enum LogColor
    {
        black = 1,
        white = 2,
        green = 3,
        cyan = 4,
        magenta = 5,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action OnGameStateChanged;

        private enum GameState
        {
            PreparingLevel,
            WaitingToStart,
            GameCountdown,
            GameActive,
            GameOver
        }

        private GameState currentState;


        
        [Header("Game Settings")]
        [Tooltip("How long we want the initial countdown to be")] [SerializeField] private float countdownTimer;
        [Tooltip("How long in seconds can the game go on")] [SerializeField] private float maxGameTime;
        private float currentGameTimer; //Current timer count



        [Header("DEVELOPMENT SETTINGS")]
        [SerializeField] private bool skipCountdownTimer;
        [SerializeField] private LogColor gameStateLogColor;

        private void Awake()
        {
            if(Instance == null){
                Instance = this;
            }
            else if (Instance != this){
                Destroy(this);
            }
            if(transform.parent) transform.parent = null;
            DontDestroyOnLoad(this);


            //DEBUG ONLY, SWITCH TO ACTIVE IMMEDIATELY
            if(skipCountdownTimer){
                ChangeGameState(GameState.GameActive);
                Debug.Log($"<color=green>We currently are in debug mode, and are skipping the countdown timer!</color>");
            }
            else{
                ChangeGameState(GameState.PreparingLevel);
            }
        }


        private void ChangeGameState(GameState state)
        {
            currentState = state;
            switch(state){
                case GameState.PreparingLevel:
                    Debug.Log($"<color={gameStateLogColor}>GAME STATE: </color>Preparing level...");
                    OnGameStateChanged?.Invoke();
                    break;
                case GameState.WaitingToStart:
                    Debug.Log($"<color={gameStateLogColor}>GAME STATE: </color>Waiting to start...");
                    OnGameStateChanged?.Invoke();
                    break;
                case GameState.GameCountdown:
                    Debug.Log($"<color={gameStateLogColor}>GAME STATE: </color>Starting countdown...");
                    OnGameStateChanged?.Invoke();
                    break;
                case GameState.GameActive:
                    Debug.Log($"<color={gameStateLogColor}>GAME STATE: </color>Game is now active...");
                    OnGameStateChanged?.Invoke();
                    break;
                case GameState.GameOver:
                    Debug.Log($"<color={gameStateLogColor}>GAME STATE: </color>Game is over...");
                    OnGameStateChanged?.Invoke();
                    break;
                default:
                    break;
            }
        }

    }
}