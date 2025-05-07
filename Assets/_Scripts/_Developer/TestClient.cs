using FishNet.Object;
using UnityEngine;

namespace ContradictiveGames
{
    public class TestClient : NetworkBehaviour
    {
        public override void OnStartClient()
        {
            base.OnStartClient();

            LobbyUIManager.OnPlayerJoined(OwnerId);
        }

        public override void OnStopClient(){
            base.OnStopClient();

            LobbyUIManager.OnPlayerLeft(OwnerId);
        }

        public override void OnStopServer(){
            base.OnStopServer();

            LobbyUIManager.OnPlayerLeft(OwnerId);
        }
    }
}
