using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerCombat : NetworkBehaviour
    {

        [SerializeField] private Transform firePoint;

        private AttackSO primaryAttack, secondaryAttack;
        private float lastPrimaryAttack, lastSecondaryAttack;

        public override void OnNetworkSpawn(){
            var player = GetComponent<Player>();
            primaryAttack = player.playerClass.PrimaryAttack;
            secondaryAttack = player.playerClass.SecondaryAttack;
        }
        

        public void DoPrimaryAttack(){
            if(Time.time > primaryAttack.AttackCooldown + lastPrimaryAttack){
                lastPrimaryAttack = Time.time;
                DoAttack(primaryAttack);
            }
        }

        public void DoSecondaryAttack(){
            if(Time.time > secondaryAttack.AttackCooldown + lastSecondaryAttack){
                lastSecondaryAttack = Time.time;
                DoAttack(secondaryAttack);
            }
        }

        private void DoAttack(AttackSO attackSO = null){
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