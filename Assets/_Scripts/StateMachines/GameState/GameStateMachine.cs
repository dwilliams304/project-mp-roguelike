using ContradictiveGames.Managers;

namespace ContradictiveGames.State
{
    public class GameStateMachine : StateMachine<GameStateMachine>
    {
        public WaitingState WaitingState;
        public CountdownState CountdownState;
        public ActiveState ActiveState;
        public RoundEndState RoundEndState;
        public GameOverState GameOverState;

        public GameManager manager { get; private set; }



        public override void InitializeStateMachine()
        {
            WaitingState = new WaitingState();
            CountdownState = new CountdownState();
            ActiveState = new ActiveState();
            RoundEndState = new RoundEndState();
            GameOverState = new GameOverState();
            
            manager = GameManager.Instance;

            ChangeState(WaitingState);
        }

        public override void ChangeState(StateNode<GameStateMachine> newState)
        {
            base.ChangeState(newState);
            manager.UpdateState(newState);
        }
    }
}