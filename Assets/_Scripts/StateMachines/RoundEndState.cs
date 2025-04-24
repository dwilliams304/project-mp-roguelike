using ContradictiveGames.Managers;

namespace ContradictiveGames
{
    public class RoundEndState : GameState
    {
        public override void StateEnter(GameManager gameManager)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered ROUND END state");
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