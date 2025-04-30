namespace ContradictiveGames.State
{
    public class PlayerAliveState : StateNode<PlayerStateMachine>
    {
        public override void OnStateEnter(PlayerStateMachine stateMachine)
        {
            CustomDebugger.Log($"<color=cyan>PLAYER STATE: </color>Player has entered ALIVE state");
        }

        public override void OnStateExit(PlayerStateMachine stateMachine)
        {
            CustomDebugger.Log($"<color=cyan>PLAYER STATE: </color>Player has exited ALIVE state");
        }

        public override void OnStateUpdate(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}