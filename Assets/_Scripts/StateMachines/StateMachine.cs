
namespace ContradictiveGames.State
{
    public abstract class StateMachine<T> where T : StateMachine<T>
    {
        public StateNode<T> currentState { get; private set; }

        public abstract void InitializeStateMachine();

        public virtual void ChangeState(StateNode<T> newState){
            currentState?.OnStateExit((T)this);
            currentState = newState;
            currentState?.OnStateEnter((T)this);
        }

        public virtual void StateUpdate(){
            currentState?.OnStateUpdate((T)this);
        }
    }
}