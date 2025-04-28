using ContradictiveGames.Managers;
using UnityEngine;

namespace ContradictiveGames
{
    public class CountdownState : GameState
    {
        public override void StateEnter(GameManager gameManager)
        {
            CustomDebugger.Log("<color=green>GAME STATE: </color>Entered COUNTDOWN state");
            
            if(gameManager.gameSettings.skipCountdownTimer){
                gameManager.ChangeGameState(gameManager.activeState, GameStateType.Active);
                return;
            }
        }

        public override void StateUpdate(GameManager gameManager)
        {
            gameManager.CurrentCountdownTimer.Value -= Time.deltaTime;
            if(gameManager.CurrentCountdownTimer.Value < 0f){
                gameManager.ChangeGameState(gameManager.activeState, GameStateType.Active);
            }
        }

        public override void StateExit(GameManager gameManager)
        {
            return;
        }
    }
}