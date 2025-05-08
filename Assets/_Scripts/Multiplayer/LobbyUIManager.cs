using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ContradictiveGames
{
    public class LobbyUIManager : MonoBehaviour
    {
        public static LobbyUIManager Instance;
        public GameObject LobbyCanvas;
        public Camera LobbyCamera;

        [SerializeField] private LobbyPlayerCard playerCardPrefab;
        [SerializeField] private Transform playerListTransform;

        private Dictionary<int, LobbyPlayerCard> players = new();

        public event Action SpawnPlayerPressed;

        private void Awake(){
            Instance = this;
        }

        public static void EnterGameButtonPressed(){
            Instance.LobbyCamera.gameObject.SetActive(false);
            Instance.SpawnPlayerPressed?.Invoke();
            Instance.LobbyCanvas.SetActive(false);
        }

        private void OnShowLobbyPanel(){
            Instance.LobbyCamera.gameObject.SetActive(true);
            Instance.LobbyCanvas.SetActive(false);
        }

        public static void OnPlayerJoined(int clientID){
            LobbyPlayerCard lpc = Instantiate(Instance.playerCardPrefab, Instance.playerListTransform);
            Instance.players.Add(clientID, lpc);
            lpc.Initialize(new UserData{
                Username = $"Player {clientID}",
                Level = 1,
                Title = $"Title {Random.Range(0, 100000)}"
            });
        }


        public static void OnPlayerLeft(int clientID){
            if(Instance.players.TryGetValue(clientID, out LobbyPlayerCard lpc)){
                if(lpc == null) return;
                Destroy(lpc.gameObject);
                Instance.players.Remove(clientID);
            }
        }
    }
}
