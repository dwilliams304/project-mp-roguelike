namespace ContradictiveGames.State
{
    public class PlayerDeadState : StateNode<PlayerStateMachine>
    {
        public override void OnStateEnter(PlayerStateMachine stateMachine)
        {
            CustomDebugger.Log($"<color=cyan>PLAYER STATE: </color>Player has entered DEAD state");
        }

        public override void OnStateExit(PlayerStateMachine stateMachine)
        {
            CustomDebugger.Log($"<color=cyan>PLAYER STATE: </color>Player has exited DEAD state");
        }

        public override void OnStateUpdate(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}