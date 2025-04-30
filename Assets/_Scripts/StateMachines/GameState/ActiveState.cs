using ContradictiveGames.Managers;
using UnityEngine;

namespace ContradictiveGames.State
{
    public class ActiveState : StateNode<GameStateMachine>
    {

        public override void OnStateEnter(GameStateMachine stateMachine)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered ACTIVE state");
            GameManager.Instance.CurrentActiveGameTimer.Value = GameManager.Instance.MaxGameTime.Value;
        }

        public override void OnStateUpdate(GameStateMachine stateMachine)
        {
            GameManager.Instance.CurrentActiveGameTimer.Value -= Time.deltaTime;
            if(GameManager.Instance.CurrentActiveGameTimer.Value < 0f){
                stateMachine.ChangeState(stateMachine.RoundEndState);
                GameManager.Instance.UpdateState(this);
            }
        }

        public override void OnStateExit(GameStateMachine stateMachine)
        {
            return;
        }
    }
}