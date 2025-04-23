using System;
using Unity.Netcode;

namespace ContradictiveGames
{
    public class BaseEntity : NetworkBehaviour, IDamageable, IHealable
    {
        public NetworkVariable<int> MaxHealth = new NetworkVariable<int>(
            100, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Server
        );
        public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(
            100,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        public event Action<int, int> MaxHealthChanged;
        public event Action<int> OnHealthChanged;

        public bool IsDamageable { get; private set; } = true;
        public bool IsHealable { get; private set; } = true;
        public bool IsStunnable { get; private set; } = false;

        private void Start()
        {
            if(IsServer){
                InitializeEntity();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(IsServer){
                InitializeEntity();
            }

            CurrentHealth.OnValueChanged += OnHealthValuesChanged;
            MaxHealth.OnValueChanged += OnMaxHealthValueChanged;
            MaxHealthChanged?.Invoke(100, 100);
        }

        public override void OnDestroy(){
            CurrentHealth.OnValueChanged -= OnHealthValuesChanged;
            MaxHealth.OnValueChanged -= OnMaxHealthValueChanged;
        }

        public void InitializeEntity()
        {
            //Set up stats, health, ui, etc
            MaxHealth.Value = 100;
            CurrentHealth.Value = 100;
        }


        public void TakeDamage(int amount)
        {
            if(!IsServer || !IsDamageable) return;
            //Do damage, sync to canvas, etc..
            CurrentHealth.Value -= amount;
            OnHealthChanged?.Invoke(CurrentHealth.Value);
            
            if(CurrentHealth.Value <= 0){
                // gameObject.SetActive(false);
                gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }

        private void OnHealthValuesChanged(int old, int newVal){
            OnHealthChanged?.Invoke(newVal);
        }
        private void OnMaxHealthValueChanged(int old, int newVal){
            MaxHealthChanged?.Invoke(newVal, CurrentHealth.Value);
        }

        
        public void TakeHeal(int amount)
        {
            if(!IsServer || !IsDamageable) return;
            //Increase health, sync to canvas/healthbar, etc
            CurrentHealth.Value += amount;
            OnHealthChanged?.Invoke(CurrentHealth.Value);
            if(CurrentHealth.Value > MaxHealth.Value){
                CurrentHealth.Value = MaxHealth.Value;
            }
        }

        private void UpdateHealthUI()
        {
            //Change health bar
        }
    }
}
