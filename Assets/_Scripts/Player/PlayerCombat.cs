using System;
using ContradictiveGames.Input;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerController), typeof(PlayerStatsHolder))]
    public class PlayerCombat : NetworkBehaviour
    {
        // private InputReader inputReader;
        private PlayerClassData playerClassData;

        public event Action<AttackSO> PrimaryAttackPerformed;
        public event Action<AttackSO> SecondaryAttackPerformed;

        private AttackSO primaryAttack, secondaryAttack;
        private float lastPrimaryAttack, lastSecondaryAttack;

        public void InitializeInputs(InputReader _inputReader){
            _inputReader.MainAttack += PrimaryAttack;
            _inputReader.SecondaryAttack += SecondaryAttack;
            _inputReader.EnablePlayerActions();
        }


        private void Awake()
        {
            playerClassData = GetComponent<PlayerStatsHolder>().playerClassData;
            primaryAttack = playerClassData.PrimaryAttack;
            secondaryAttack = playerClassData.SecondaryAttack;
        }

        private void PrimaryAttack(){
            if(Time.time > primaryAttack.AttackCooldown + lastPrimaryAttack){
                lastPrimaryAttack = Time.time;
                DoAttack(primaryAttack);
                PrimaryAttackPerformed?.Invoke(primaryAttack);
            }
        }

        private void SecondaryAttack(){
            if(Time.time > secondaryAttack.AttackCooldown + lastSecondaryAttack){
                lastSecondaryAttack = Time.time;
                DoAttack(secondaryAttack);
                SecondaryAttackPerformed?.Invoke(secondaryAttack);
            }
        }

        private void DoAttack(AttackSO attackSO){
            switch(attackSO.attackType){
                case AttackType.Melee:
                    Debug.Log("Do melee attack!");
                    break;
                case AttackType.Ranged:
                    Debug.Log("Do ranged attack!");
                    break;
            }
        }
        
    }
}