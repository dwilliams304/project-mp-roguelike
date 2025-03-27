using UnityEngine;
using Unity.Netcode;
using ContradictiveGames.Interfaces;
using ContradictiveGames.Systems.Stats;
using System;

namespace ContradictiveGames.Systems.Health
{
    public class Health : NetworkBehaviour, IDamageable, IHealable
    {
        private Stat MaxHealth;
        public int CurrentHealth { get; private set; }


        public bool CanTakeDamage { get; private set; }
        public bool CanHeal { get; private set; }

        public event Action OnTakeDamage;
        public event Action OnDeath;



        public override void OnNetworkSpawn()
        {
            CurrentHealth = MaxHealthAsInt();
        }

        public void TakeDamage(int amount){
            if(CanTakeDamage){
                CurrentHealth -= amount;
                OnTakeDamage?.Invoke();
                if(CurrentHealth <= 0){
                    Die();
                }
            }
        }

        public void TakeHeal(int amount)
        {
            if(CanHeal){
                CurrentHealth += amount;
                if(CurrentHealth >= MaxHealth.Value){
                    CurrentHealth = MaxHealthAsInt();
                    Debug.Log("Over healed!", this);
                }
            }
        }

        /// <summary>
        /// Add a modifier to our current max health
        /// </summary>
        /// <param name="modifier">Modifier to add</param>
        /// <param name="setCurrentToMax">If we want to set the current health to our new max health amount</param>
        public void AddModifierToMaxHealth(StatModifier modifier, bool setCurrentToMax = false){
            if(!MaxHealth.canNotAugment){
                MaxHealth.AddModifier(modifier);
                if(setCurrentToMax){
                    CurrentHealth = MaxHealthAsInt();
                }
            }
        }

        public void RemoveModifierFromMaxHealth(StatModifier modifier){
            if(!MaxHealth.canNotAugment){
                MaxHealth.RemoveModifier(modifier);
                if(CurrentHealth > MaxHealthAsInt()) CurrentHealth = MaxHealthAsInt();
            }
        }

        private void Die(){
            Debug.Log("Entity has died!", this);
            OnDeath?.Invoke();
        }

        private int MaxHealthAsInt(){
            return Mathf.RoundToInt(MaxHealth.Value);
        }


    }
}
