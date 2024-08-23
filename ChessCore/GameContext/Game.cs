using ChessCore.FindValidMoves;
using ChessCore.Moves;

namespace ChessCore.GameContext
{
    public class Game
    {
        private State state;
        
        private Result result = Result.InProgress;

        private readonly Player whitePlayer;
        private readonly Player blackPlayer;
    
        public Game(Player whitePlayer, Player blackPlayer, string? fenString = null) 
        {
            state = fenString is null ? new State() : new State(fenString);

            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;

            whitePlayer.OnGameStarted(this, Piece.White);
            blackPlayer.OnGameStarted(this, Piece.Black);
        }

        public void Start()
        {
            whitePlayer.OnPlayerTurn();
        }

        public State GetState()
        {
            return state;
        }

        public Result GetGameResult()
        {
            return result;
        }

        public void PlayMove(int move)
        {
            MoveHelper.ExecuteMove(state, move);

            whitePlayer.OnMovePlayed(move);
            blackPlayer.OnMovePlayed(move);

            if (IsGameOver()) return;
            
            Player player = state.whiteToPlay ? whitePlayer : blackPlayer;
            player.OnPlayerTurn();
        }

        private bool IsGameOver()
        {
            if (ValidMovesFinder.FindAllMoves(state).Count != 0) return false;
            
            state.whiteToPlay = !state.whiteToPlay;
            if (ValidState.IsStateValid(state))
            {
                result = Result.Draw;
            }
            else
            {
                result = state.whiteToPlay ? Result.WhiteWin : Result.BlackWin;
            }

            return true;
        }
    }
}
