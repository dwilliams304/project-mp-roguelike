using System.Collections.Generic;
using UnityEngine;

namespace ContradictiveGames
{
    public class LobbyUIManager : MonoBehaviour
    {
        public static LobbyUIManager Instance;
        [SerializeField] private LobbyPlayerCard playerCardPrefab;
        [SerializeField] private Transform playerListTransform;

        private Dictionary<int, LobbyPlayerCard> players = new();

        private void Awake(){
            Instance = this;
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
                Destroy(lpc.gameObject);
                Instance.players.Remove(clientID);
            }
        }
    }
}
