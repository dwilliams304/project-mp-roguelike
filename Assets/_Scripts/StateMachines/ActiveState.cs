using ContradictiveGames.Managers;
using UnityEngine;

namespace ContradictiveGames
{
    public class ActiveState : GameState
    {
        public override void StateEnter(GameManager gameManager)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered ACTIVE state");
           gameManager.currentActiveGameTimer.Value = gameManager.MaxGameTime;
        }

        public override void StateUpdate(GameManager gameManager)
        {
            gameManager.currentActiveGameTimer.Value -= Time.deltaTime;
            if(gameManager.currentActiveGameTimer.Value < 0f){
                gameManager.ChangeGameState(gameManager.roundEndState, GameStateType.RoundEnd);
            }
        }

        public override void StateExit(GameManager gameManager)
        {
            return;
        }
    }
}