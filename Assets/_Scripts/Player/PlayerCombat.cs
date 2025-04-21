using ContradictiveGames.Input;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class PlayerCombat : NetworkBehaviour
    {

        private Transform firePoint;
        private InputReader inputReader;


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
        
        public void InitializeStats(PlayerStats playerStats){

        }

        private void Deinitialize(){
            if(inputReader != null){
                inputReader.MainAttack -= MainAttack;
                inputReader.SecondaryAttack -= SecondaryAttack;
            }
        }

        private bool CanDoPrimary(){
            return false;
        }
        private bool CanDoSecondary(){
            return false;
        }

        private void MainAttack(){

        }
        private void SecondaryAttack(){

        }
    }
}