using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerCombat : NetworkBehaviour
    {

        [SerializeField] private Transform firePoint;


        public override void OnNetworkSpawn(){
            var player = GetComponent<Player>();
        }
        

        private bool CanDoPrimary(){
            return false;
        }
        private bool CanDoSecondary(){
            return false;
        }
    }
}