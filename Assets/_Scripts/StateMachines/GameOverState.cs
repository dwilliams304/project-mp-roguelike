using ContradictiveGames.Managers;

namespace ContradictiveGames
{
    public class GameOverState : GameState
    {
        public override void StateEnter(GameManager gameManager)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered GAME OVER state");
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