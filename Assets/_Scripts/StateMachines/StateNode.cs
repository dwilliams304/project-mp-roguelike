namespace ContradictiveGames.State
{
    public abstract class StateNode<T>
    {
        public virtual void OnStateEnter(T stateMachine){

        }
        public virtual void OnStateUpdate(T stateMachine){

        }
        public virtual void OnStateExit(T stateMachine){

        }
    }
}