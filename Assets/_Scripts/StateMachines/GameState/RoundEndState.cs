using ContradictiveGames.Managers;

namespace ContradictiveGames.State
{
    public class RoundEndState : StateNode<GameStateMachine>
    {
        public override void OnStateEnter(GameStateMachine stateMachine)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered ROUND END state");
            return;
        }

        public override void OnStateUpdate(GameStateMachine stateMachine)
        {
            return;
        }

        public override void OnStateExit(GameStateMachine stateMachine)
        {
            return;
        }
    }
}