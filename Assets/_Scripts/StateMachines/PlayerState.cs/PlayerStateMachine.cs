using UnityEngine;

namespace ContradictiveGames.State
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private StateNode<PlayerStateMachine> currentState;
        public PlayerAliveState PlayerAliveState = new();
        public PlayerDeadState PlayerDeadState = new();
        public PlayerRespawningState PlayerRespawningState = new();


        
    }
}