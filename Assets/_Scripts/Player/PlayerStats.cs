using System.Collections;
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


        [ServerRpc]
        private void InitializeStatsServerRpc(int maxHealth)
        {
            MaxHealth.Value = maxHealth;
            CurrentHealth.Value = maxHealth;
            CurrentLevel.Value = 1;
            XpToNextLevel.Value = 500;
            CurrentXP.Value = 0;
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
