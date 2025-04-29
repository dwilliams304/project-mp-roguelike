using System.Collections;
using ContradictiveGames.Managers;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class PlayerStats : NetworkBehaviour, IDamageable
    {
        private PlayerOverlayUIController playerOverlayUIController;

        public NetworkVariable<int> MaxHealth = new();
        public NetworkVariable<int> CurrentHealth = new();
        public NetworkVariable<int> CurrentLevel = new();
        public NetworkVariable<int> XpToNextLevel = new();
        public NetworkVariable<int> CurrentXP = new();

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
            GameManager.Instance.GameStateChanged += GameStart;
        }

        public override void OnDestroy(){
            GameManager.Instance.GameStateChanged -= GameStart;
        }


        [ServerRpc]
        private void InitializeStatsServerRpc(int maxHealth)
        {
            MaxHealth.Value = maxHealth;
            CurrentHealth.Value = maxHealth;
            CurrentLevel.Value = 1;
            XpToNextLevel.Value = 500;
            CurrentXP.Value = 0;
        }

        private void GameStart(GameStateType state){
            if(state == GameStateType.Active){
                RequestToChangeHealthServerRpc(MaxHealth.Value);
            }
        }
        [ServerRpc(RequireOwnership = false)]
        private void RequestToChangeHealthServerRpc(int amount){
            CurrentHealth.Value = amount;
        }

        public void TakeDamage(int amount)
        {
            RequestToTakeDamageServerRpc(amount);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestToTakeDamageServerRpc(int amount)
        {
            CurrentHealth.Value = Mathf.Max(CurrentHealth.Value - amount, 0); // Prevent negative health
        }
    }
}
