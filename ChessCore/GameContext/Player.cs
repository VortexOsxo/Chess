using ChessCore.Moves;
using System.Diagnostics;

namespace ChessCore.GameContext
{
    public abstract class Player
    {
        protected Game game;

        public State? state;
        public int color;
        public int gameId;

        public virtual void OnGameStarted(Game game, int color)
        {
            this.game = game;

            this.state = new State(game.GetState());
            this.color = color;
        }

        public virtual void OnMovePlayed(int move)
        {
            Debug.Assert(state != null);

            MoveHelper.ExecuteMove(state, move);
        }

        public virtual void OnGameEnded(Result result) { }


        public abstract void OnPlayerTurn();
    }
}
