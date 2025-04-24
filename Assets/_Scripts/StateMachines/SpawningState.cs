using ContradictiveGames.Managers;

namespace ContradictiveGames
{
    public class SpawningState : GameState
    {
        public override void StateEnter(GameManager gameManager)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered SPAWNING state");
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