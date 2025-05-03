
namespace ContradictiveGames.State
{
    public class GameOverState : StateNode<GameStateMachine>
    {
        public override void OnStateEnter(GameStateMachine stateMachine)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered GAME OVER state");
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