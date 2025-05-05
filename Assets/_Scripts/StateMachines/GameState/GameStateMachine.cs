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

        protected GameManager manager;



        public override void InitializeStateMachine()
        {
            WaitingState = new WaitingState();
            CountdownState = new CountdownState();
            ActiveState = new ActiveState();
            RoundEndState = new RoundEndState();
            GameOverState = new GameOverState();
            

            ChangeState(WaitingState);
        }

        public override void ChangeState(StateNode<GameStateMachine> newState)
        {
            base.ChangeState(newState);
        }
    }
}