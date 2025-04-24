using ContradictiveGames.Managers;


namespace ContradictiveGames
{
    public abstract class GameState
    {
        public abstract void StateEnter(GameManager gameManager);
        public abstract void StateUpdate(GameManager gameManager);
        public abstract void StateExit(GameManager gameManager);
    }
}
