using FishNet.Object;
using UnityEngine;

namespace ContradictiveGames.Entities
{
    public class EnemyNPC : NetworkBehaviour, IEntity, IHealth
    {
        [Header("Entity Setup")]
        [SerializeField] private EntityData _entityData;
        [SerializeField] private EntityUIController _entityUIController;

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;


        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;

        public EntityData EntityData => _entityData;
        public EntityUIController entityUIController => _entityUIController;


        public override void OnStartClient()
        {
            base.OnStartClient();
            _currentHealth = _maxHealth;
            if(_entityUIController == null){
                _entityUIController = GetComponentInChildren<EntityUIController>();
                if(_entityUIController == null){
                    CustomDebugger.Log("No entity UI controller found!");
                }
            }
        }

        
        public void InitializeUI(EntityData data)
        {
            throw new System.NotImplementedException();
        }



        public bool IsHealable()
        {
            return true;
        }

        
        public void Heal(int amount)
        {
            HealServer(amount);
        }
        [ServerRpc(RequireOwnership = false)]
        private void HealServer(int amount){
            if(!IsHealable()) return;
            
            _currentHealth += amount;
            if(_currentHealth > _maxHealth){
                _currentHealth = _maxHealth;
            }
            
            if(entityUIController != null) entityUIController.UpdateHealthBarCurrent(_currentHealth);

        }


        public bool IsDamageable()
        {
            return true;
        }

        public void Damage(int amount)
        {
            DamageServer(amount);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DamageServer(int amount){
            if(!IsDamageable()) return;

            _currentHealth -= amount;
            if(_currentHealth <= 0){
                Die();
            }
        
            if(entityUIController != null) entityUIController.UpdateHealthBarCurrent(_currentHealth);
        }


        [Server]
        public virtual void Die()
        {
            ServerManager.Despawn(gameObject);
            CustomDebugger.Log($"We died! ({gameObject.name})");
        }


    }
}