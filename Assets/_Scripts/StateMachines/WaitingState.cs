using ContradictiveGames.Managers;

namespace ContradictiveGames
{
    public class WaitingState : GameState
    {
        public override void StateEnter(GameManager gameManager)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered WAITING state");
            return;
        }

        public override void StateUpdate(GameManager gameManager)
        {
            return;
        }

        public override void StateExit(GameManager gameManager)
        {
            return;
        }
    }
}