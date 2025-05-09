using ContradictiveGames.Managers;
using UnityEngine;

namespace ContradictiveGames.State
{
    public class CountdownState : StateNode<GameStateMachine>
    {
        public override void OnStateEnter(GameStateMachine stateMachine)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered COUNTDOWN state");
        }

        public override void OnStateUpdate(GameStateMachine stateMachine)
        {
            stateMachine.manager.CurrentCountdownTimer.Value -= Time.deltaTime;
            if(stateMachine.manager.CurrentCountdownTimer.Value < 0f){
                stateMachine.ChangeState(stateMachine.ActiveState);
            }
        }

        public override void OnStateExit(GameStateMachine stateMachine)
        {
            return;
        }
    }
}