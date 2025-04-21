using System;
using UnityEngine;

namespace ContradictiveGames.Managers
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Settings")]
        [Tooltip("How long we want the initial countdown to be")] [SerializeField] private float countdownTimer;
        [Tooltip("How long in seconds can the game go on")] [SerializeField] private float maxGameTime;
        private float currentGameTimer; //Current timer count


        [Header("DEVELOPMENT SETTINGS")]
        [SerializeField] private bool skipCountdownTimer;

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


        }



    }
}