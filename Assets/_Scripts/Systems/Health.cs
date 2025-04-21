using UnityEngine;
using Unity.Netcode;
using System;
using ContradictiveGames.Player;

namespace ContradictiveGames.Systems.Health
{
    public class Health : NetworkBehaviour, IDamageable, IHealable
    {
        [SerializeField] private NetworkVariable<int> maxHealth = new();
        [SerializeField] private NetworkVariable<int> health = new();
        

        private bool canHeal, canTakeDamage;

        public event Action<int> OnChangeHealth;
        public event Action OnDeath;


        public void Init(PlayerClassData playerClassData)
        {
            maxHealth.Value = playerClassData.MaxHealth;
            health.Value = maxHealth.Value;
            OnChangeHealth?.Invoke(health.Value);
        }


        public void TakeDamage(int amount)
        {
            if(canTakeDamage){
                //Do something
                health.Value -= amount;
                if(health.Value <= 0)
                {
                    OnDeath?.Invoke();
                }
                OnChangeHealth?.Invoke(health.Value);
            }
        }
        public void TakeHeal(int amount)
        {
            if(canHeal){
                //Do something
                health.Value += amount;
                if(health.Value > maxHealth.Value) health.Value = maxHealth.Value;
                OnChangeHealth?.Invoke(health.Value);
            }
        }


        public void ToggleDamageable(bool toggleTo) => canTakeDamage = toggleTo;
        public void ToggleHealable(bool toggleTo) => canHeal = toggleTo;

    }
}
