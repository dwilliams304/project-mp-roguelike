using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace ContradictiveGames
{
    public class BaseEntity : NetworkBehaviour
    {
        public readonly SyncVar<int> MaxHealth;
        public readonly SyncVar<int> CurrentHealth;

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
            Canvas worldSpaceCanvas = GetComponentInChildren<Canvas>();
            if(worldSpaceCanvas != null) worldSpaceCanvas.worldCamera = Camera.main;
        }

        // public override void OnNetworkSpawn()
        // {
        //     if(IsServer){
        //         InitializeEntity();
        //     }

        //     CurrentHealth.OnChange += OnHealthValuesChanged;
        //     MaxHealth.OnChange += OnMaxHealthValueChanged;
        //     MaxHealthChanged?.Invoke(100, 100);
        // }


        // public override void OnDestroy(){
        //     CurrentHealth.OnChange -= OnHealthValuesChanged;
        //     MaxHealth.OnChange -= OnMaxHealthValueChanged;
        // }

        public void InitializeEntity()
        {
            //Set up stats, health, ui, etc
            // MaxHealth.Value = 100;
            // CurrentHealth.Value = 100;
        }



        private void OnHealthValuesChanged(int oldVal, int newVal, bool asServer){
            OnHealthChanged?.Invoke(newVal);
        }
        private void OnMaxHealthValueChanged(int old, int newVal, bool asServer){
            MaxHealthChanged?.Invoke(newVal, CurrentHealth.Value);
        }

        


        private void UpdateHealthUI()
        {
            //Change health bar
        }
    }
}
