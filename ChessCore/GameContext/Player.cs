namespace ChessCore.GameContext
{
    public abstract class Player
    {
        public virtual void OnGameStarted(Game game, int color) { }
        public virtual void OnGameEnded(Result result) { }


        public abstract void OnPlayerTurn();
    }
}
