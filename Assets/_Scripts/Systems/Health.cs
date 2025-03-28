using UnityEngine;
using Unity.Netcode;
using ContradictiveGames.Interfaces;
using ContradictiveGames.Systems.Stats;
using System;

namespace ContradictiveGames.Systems.Health
{
    public class Health : NetworkBehaviour, IDamageable, IHealable
    {
        public void TakeDamage(int amount)
        {
            throw new NotImplementedException();
        }

        public void TakeHeal(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
