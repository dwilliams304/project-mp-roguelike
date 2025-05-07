using ContradictiveGames.Entities;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class PlayerEntity : NetworkBehaviour, IEntity, IHealth, IEffectable
    {

        [SerializeField] private EntityData _entityData;
        [SerializeField] private EntityUIController _entityUIController; 
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;


        public EntityUIController entityUIController => _entityUIController;
        public EntityData EntityData => _entityData;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public override void OnStartClient()
        {
            base.OnStartClient();
            _maxHealth = 100;
            _currentHealth = _maxHealth;
            if(_entityUIController == null) {
                _entityUIController = GetComponentInChildren<EntityUIController>();
                if(_entityUIController == null){
                    CustomDebugger.LogError("No EntityUIController setup", ConsoleLogLevel.InDepth);
                    return;
                }
            }

            if(IsOwner){
                PlayerOverlayUIController.LocalInstance.UpdateCurrentMax(_maxHealth, true);
                PlayerOverlayUIController.LocalInstance.UpdateLevelText(1, 500, 23);
            }
            entityUIController.UpdateHealthBarMax(MaxHealth, true);
        }
        public void InitializeUI(EntityData data)
        {
            throw new System.NotImplementedException();
        }


        public bool IsEffectable()
        {
            return true;
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddEffectServerRpc()
        {
            throw new System.NotImplementedException();
        }


        public bool IsDamageable()
        {
            return true;
        }

        public void Damage(int amount){
            DamageServer(amount);
        }
        [ServerRpc(RequireOwnership = false)]
        public void DamageServer(int amount)
        {
            if(!IsDamageable()) return;
            _currentHealth -= amount;
            if(entityUIController != null) {
                entityUIController.UpdateHealthBarCurrent(_currentHealth);
            }

            if(IsOwner){
                PlayerOverlayUIController.LocalInstance.UpdateCurrentHealth(_currentHealth);
            }

            if(_currentHealth <= 0){
                _currentHealth = 0;
                Die();
            }
        }

        
        public bool IsHealable()
        {
            return true;
        }
        
        public void Heal(int amount){
            HealServer(amount);
        }
        [ServerRpc(RequireOwnership = false)]
        public void HealServer(int amount)
        {
            if(!IsHealable()) return;
            _currentHealth += amount;
            if(_currentHealth > _maxHealth){
                _currentHealth = _maxHealth;
            }
            if(entityUIController != null) entityUIController.UpdateHealthBarCurrent(_currentHealth);
            if(IsOwner){
                PlayerOverlayUIController.LocalInstance.UpdateCurrentHealth(_currentHealth);
            }
        }


        [Server]
        public void Die()
        {
            CustomDebugger.LogError("We died!", ConsoleLogLevel.InDepth);
        }
    }
}