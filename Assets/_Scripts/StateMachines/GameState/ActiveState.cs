using ContradictiveGames.Managers;
using UnityEngine;

namespace ContradictiveGames.State
{
    public class ActiveState : StateNode<GameStateMachine>
    {

        public override void OnStateEnter(GameStateMachine stateMachine)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered ACTIVE state");
            
        }

        public override void OnStateUpdate(GameStateMachine stateMachine)
        {
            stateMachine.manager.CurrentActiveGameTimer.Value -= Time.deltaTime;
            if(stateMachine.manager.CurrentActiveGameTimer.Value < 0f){
                stateMachine.ChangeState(stateMachine.RoundEndState);
            }
        }

        public override void OnStateExit(GameStateMachine stateMachine)
        {
            return;
        }
    }
}