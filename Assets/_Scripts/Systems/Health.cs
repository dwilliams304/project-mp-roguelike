using UnityEngine;
using Unity.Netcode;
using ContradictiveGames.Player;

namespace ContradictiveGames.Systems.Health
{
    public class Health : NetworkBehaviour, IDamageable, IHealable
    {
        [SerializeField] private NetworkVariable<int> maxHealth = new();
        [SerializeField] private NetworkVariable<int> health = new();

        

        private bool canHeal, canTakeDamage;


        public void Init(PlayerStats playerStats)
        {
            maxHealth.Value = playerStats.MaxHealth.ValueAsInt;
            health.Value = maxHealth.Value;
        }


        public void TakeDamage(int amount)
        {
            if(canTakeDamage){
                //Do something
                health.Value -= amount;
                if(health.Value <= 0)
                {
                    Debug.Log("We died!");
                }
                
            }
        }
        public void TakeHeal(int amount)
        {
            if(canHeal){
                //Do something
                health.Value += amount;
                if(health.Value > maxHealth.Value) health.Value = maxHealth.Value;
                
            }
        }


        public void ToggleDamageable(bool toggleTo) => canTakeDamage = toggleTo;
        public void ToggleHealable(bool toggleTo) => canHeal = toggleTo;

    }
}
