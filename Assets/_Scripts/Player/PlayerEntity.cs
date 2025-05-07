using System;
using ContradictiveGames.Entities;
using ContradictiveGames.Managers;
using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class PlayerEntity : NetworkBehaviour, IEntity, IHealth, IEffectable
    {

        [SerializeField] private EntityData _entityData;
        [SerializeField] private EntityUIController _entityUIController;

        [AllowMutableSyncType]
        public SyncVar<int> MaxHealth;
        [AllowMutableSyncType]
        public SyncVar<int> CurrentHealth;


        public EntityUIController entityUIController => _entityUIController;
        public EntityData EntityData => _entityData;

        public override void OnStartServer()
        {
            base.OnStartServer();

            MaxHealth.Value = 100;
            CurrentHealth.Value = MaxHealth.Value;

            entityUIController.UpdateHealthBarMax(MaxHealth.Value, true);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            MaxHealth.OnChange += UpdateMaxHealthUI;
            CurrentHealth.OnChange += UpdateCurrentHealthUI;
            if(_entityUIController == null) {
                _entityUIController = GetComponentInChildren<EntityUIController>();
                if(_entityUIController == null){
                    CustomDebugger.LogError("No EntityUIController setup", ConsoleLogLevel.InDepth);
                    return;
                }
                entityUIController.SetPlayerColors(!IsOwner);
            }

            if(IsOwner){
                PlayerOverlayUIController.LocalInstance.UpdateCurrentMax(MaxHealth.Value, true);
                PlayerOverlayUIController.LocalInstance.UpdateLevelText(1, 500, 23);
                entityUIController.SetPlayerColors(false);
            }
            else{
                entityUIController.SetPlayerColors(true);
            }

            

            GameManager.Instance.CurrentGameStateType.OnChange += CheckGameState;
            entityUIController.UpdateHealthBarMax(MaxHealth.Value, true);
        }


        public override void OnStopClient(){
            base.OnStopClient();
            MaxHealth.OnChange -= UpdateMaxHealthUI;
            CurrentHealth.OnChange -= UpdateCurrentHealthUI;
            GameManager.Instance.CurrentGameStateType.OnChange -= CheckGameState;
        }

        private void CheckGameState(GameStateType prev, GameStateType next, bool asServer)
        {
            switch(next){
                case GameStateType.Waiting:
                    if(IsOwner) entityUIController.SetPlayerColors(false);
                    else entityUIController.SetPlayerColors(true);
                    break;
                case GameStateType.Countdown:
                    if(!IsOwner) entityUIController.SetPlayerColors(false);
                    break;
                case GameStateType.Active:
                    if(!IsOffline) entityUIController.SetPlayerColors(false);
                    break;
            }
        }
        
        private void UpdateCurrentHealthUI(int prevValue, int newValue, bool asServer)
        {
            entityUIController.UpdateHealthBarCurrent(newValue);
            if(IsOwner){
                PlayerOverlayUIController.LocalInstance.UpdateCurrentHealth(newValue);
            }
        }

        private void UpdateMaxHealthUI(int prevValue, int newValue, bool asServer)
        {
            entityUIController.UpdateHealthBarMax(newValue);
            if(IsOwner){
                PlayerOverlayUIController.LocalInstance.UpdateCurrentMax(newValue);
            }
        }

        public void InitializeUI(EntityData data)
        {
            throw new System.NotImplementedException();
        }

        public int GetMaxHealth() => MaxHealth.Value;
        public int GetCurrentHealth() => CurrentHealth.Value;


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

        [ServerRpc(RequireOwnership = false)]
        public void Damage(int amount){
            if(!IsDamageable()) return;
            CurrentHealth.Value -= amount;


            if(CurrentHealth.Value <= 0){
                CurrentHealth.Value = 0;
                Die();
            }
        }

        
        public bool IsHealable()
        {
            return true;
        }

        [ServerRpc(RequireOwnership = false)]
        public void Heal(int amount){
            if(!IsHealable()) return;
            CurrentHealth.Value += amount;
            if(CurrentHealth.Value > GetMaxHealth()){
                CurrentHealth.Value = GetMaxHealth();
            }
        }


        [Server]
        public void Die()
        {
            CustomDebugger.LogError("We died!", ConsoleLogLevel.InDepth);
        }
    }
}