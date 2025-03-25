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

        public void InitializeInputs(InputReader _inputReader){
            _inputReader.MainAttack += PrimaryAttack;
            _inputReader.SecondaryAttack += SecondaryAttack;
            _inputReader.EnablePlayerActions();
        }


        private void Awake()
        {
            playerClassData = GetComponent<PlayerStatsHolder>().playerClassData;
        }

        private void PrimaryAttack(){
            playerClassData.PrimaryAttack.OnAttackPerformed();
        }

        private void SecondaryAttack(){
            playerClassData.SecondaryAttack.OnAttackPerformed();
        }
        
    }
}