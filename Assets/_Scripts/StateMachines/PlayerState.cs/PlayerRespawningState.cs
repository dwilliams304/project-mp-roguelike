namespace ContradictiveGames.State
{
    public class PlayerRespawningState : StateNode<PlayerStateMachine>
    {
        public override void OnStateEnter(PlayerStateMachine stateMachine)
        {
            CustomDebugger.Log($"<color=cyan>PLAYER STATE: </color>Player has entered RESPAWNING state");
        }

        public override void OnStateExit(PlayerStateMachine stateMachine)
        {
            CustomDebugger.Log($"<color=cyan>PLAYER STATE: </color>Player has exited RESPAWNING state");
        }

        public override void OnStateUpdate(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}