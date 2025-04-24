using ContradictiveGames.Managers;
using UnityEngine;

namespace ContradictiveGames
{
    public class CountdownState : GameState
    {
        public override void StateEnter(GameManager gameManager)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered COUNTDOWN state");
            return;
        }

        public override void StateExit(GameManager gameManager)
        {
            return;
        }

        public override void StateUpdate(GameManager gameManager)
        {
            gameManager.currentCountdownTimer.Value -= Time.deltaTime;
            if(gameManager.currentCountdownTimer.Value < 0f){
                gameManager.ChangeGameState(gameManager.activeState, GameStateType.Active);
            }
        }
    }
}