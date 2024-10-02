using UnityEngine;
using Mirror;
using Steamworks;
using ContradictiveGames.Multiplayer.Managers;
using TMPro;

namespace ContradictiveGames.Multiplayer
{
    public class SteamLobby : MonoBehaviour
    {
        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> JoinRequest;
        protected Callback<LobbyEnter_t> LobbyEntered;

        public ulong CurrentLobbyID;
        private const string HostAddressKey = "HostAddress";
        private CustomNetworkManager manager;

        public GameObject HostButton;
        public TMP_Text LobbyNameText;

        private void Start(){
            if(!SteamManager.Initialized) return;

            manager = GetComponent<CustomNetworkManager>();

            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }


        public void HostLobby(){
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
        }


        private void OnLobbyCreated(LobbyCreated_t callback){
            if(callback.m_eResult != EResult.k_EResultOK) return;
            Debug.Log("Lobby created Succesffuly");

            manager.StartHost();

            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback){
            Debug.Log("Request to join lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback){
            HostButton.SetActive(false);
            CurrentLobbyID = callback.m_ulSteamIDLobby;
            LobbyNameText.gameObject.SetActive(true);
            LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");


            if(NetworkServer.active) return;

            manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            manager.StartClient();
        }
    }
}