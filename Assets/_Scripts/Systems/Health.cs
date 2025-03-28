using UnityEngine;
using Unity.Netcode;
using ContradictiveGames.Interfaces;
using ContradictiveGames.Systems.Stats;
using System;

namespace ContradictiveGames.Systems.Health
{
    public class Health : NetworkBehaviour, IDamageable, IHealable
    {
        [SerializeField] private NetworkVariable<int> health = new(100);
        public int CurrentHealth => health.Value;

        private bool canHeal, canTakeDamage;


        public void AllowDamage() => canTakeDamage = true;
        public void DisallowDamage() => canTakeDamage = false;

        public void AllowHeals() => canHeal = true;
        public void DisallowHeals() => canHeal = false;


        public void TakeDamage(int amount)
        {
            if(canTakeDamage){
                //Do something
            }
        }
        public void TakeHeal(int amount)
        {
            if(canHeal){
                //Do something
            }
        }
    }
}
