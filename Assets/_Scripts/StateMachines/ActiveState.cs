using ContradictiveGames.Managers;
using UnityEngine;

namespace ContradictiveGames
{
    public class ActiveState : GameState
    {
        public override void StateEnter(GameManager gameManager)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered ACTIVE state");
            gameManager.CurrentActiveGameTimer.Value = gameManager.MaxGameTime.Value;
        }

        public override void StateUpdate(GameManager gameManager)
        {
            gameManager.CurrentActiveGameTimer.Value -= Time.deltaTime;
            if(gameManager.CurrentActiveGameTimer.Value < 0f){
                gameManager.ChangeGameState(gameManager.roundEndState, GameStateType.RoundEnd);
            }
        }

        public override void StateExit(GameManager gameManager)
        {
            return;
        }
    }
}