using ContradictiveGames.Managers;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class PlayerStats : NetworkBehaviour
    {
        private PlayerOverlayUIController playerOverlayUIController;

        public readonly SyncVar<int> MaxHealth;
        public readonly SyncVar<int> CurrentHealth;
        public readonly SyncVar<int> CurrentLevel;
        public readonly SyncVar<int> XpToNextLevel;
        public readonly SyncVar<int> CurrentXP;

        public void InitializeStats(PlayerClassData playerClassData)
        {

            if (IsOwner)
            {

                InitializeStatsServerRpc(playerClassData.MaxHealth);

                playerOverlayUIController = PlayerOverlayUIController.LocalInstance;
                playerOverlayUIController.InitializeUI(this);
            }
        }

        private void Start(){
            // GameManager.Instance.GameStateChanged += GameStart;
        }

        public void OnDisable(){
            // GameManager.Instance.GameStateChanged -= GameStart;
        }


        [ServerRpc]
        private void InitializeStatsServerRpc(int maxHealth)
        {
            // MaxHealth.Value = maxHealth;
            // CurrentHealth.Value = maxHealth;
            // CurrentLevel.Value = 1;
            // XpToNextLevel.Value = Mathf.RoundToInt(GameManager.Instance.GetXPScaler(1));
            // CurrentXP.Value = 0;
        }

        private void GameStart(GameStateType state){
            if(state == GameStateType.Active){
                
            }
        }

    }
}
