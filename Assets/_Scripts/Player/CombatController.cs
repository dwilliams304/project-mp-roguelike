using ContradictiveGames.Input;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class CombatController : NetworkBehaviour
    {

        private Transform firePoint;
        private InputReader inputReader;

        private AttackData primaryAttack;
        private AttackData secondaryAttack;

        public float lastPrimaryAttack;
        public float lastSecondaryAttack;


        public override void OnNetworkDespawn()
        {
            if(IsOwner){
                Deinitialize();
            }
        }

        public void SetUpInput(InputReader _inputReader, Transform _firePoint){
            inputReader = _inputReader;
            firePoint = _firePoint;

            if(inputReader != null){
                inputReader.MainAttack += MainAttack;
                inputReader.SecondaryAttack += SecondaryAttack;
            }
        }
        
        public void InitializeCombatController(PlayerClassData classData){
            primaryAttack = classData.PrimaryAttack;
            secondaryAttack = classData.SecondaryAttack;
        }

        private void Deinitialize(){
            if(inputReader != null){
                inputReader.MainAttack -= MainAttack;
                inputReader.SecondaryAttack -= SecondaryAttack;
            }
        }

        private bool CanDoPrimary(){
            return true;
        }
        private bool CanDoSecondary(){
            return true;
        }

        private void MainAttack(){
            if(CanDoPrimary()){

            }
        }

        private void SecondaryAttack(){
            if(CanDoSecondary()){

            }
        }

        
    }
}