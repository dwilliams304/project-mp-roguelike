using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace ContradictiveGames.Entities
{
    public class Entity : NetworkBehaviour, IEntity
    {
        [Header("Entity Stats")]
        [SerializeField] private EntityData _entityData;

        [SerializeField] private int _maxHealth;
        private int _currentHealth;

        public EntityData EntityData { 
            get => _entityData; 
        }
        public int MaxHealth { 
            get => _maxHealth;
        }
        public int CurrentHealth { 
            get => _currentHealth; 
        }

        [ServerRpc(RequireOwnership = false)]
        public void DamageServerRpc(int amount)
        {
            _currentHealth -= amount;
            if(_currentHealth <= 0) Die();
        }

        [ServerRpc(RequireOwnership = false)]
        public void HealServerRpc(int amount)
        {
            _currentHealth += amount;
            if(_currentHealth > _maxHealth) _currentHealth = _maxHealth;
        }


        //TEMPORARY
        public bool IsDamageable() => _entityData.IsDamageable;
        public bool IsHealable() => _entityData.IsHealable;


        [Server]
        public void Die()
        {
            ServerManager.Despawn(gameObject);
            CustomDebugger.Log($"We died! ({gameObject.name})");
        }
    }
}