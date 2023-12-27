using ChessCore.FindValidMoves;
using ChessCore.Moves;
using ChessAI;

namespace ChessContext
{
    public class Game
    {
        public enum Result
        {
            InProgress,
            Draw,
            WhiteWin,
            BlackWin,
        }

        private ChessCore.State state;
        private ValidMovesFinder finder;
        private Result result = Result.InProgress;
    
        public Game(string fenString = null) 
        {
            state = fenString is null ? new ChessCore.State() : new ChessCore.State(fenString);
            finder = new ValidMovesFinder(state);
        }

        public int[] GetBoard()
        {
            return state.board;
        }

        public int GetPiece(int position)
        {
            return state.board[position];
        }
    
        public int GetTeamToPlay()
        {
            return state.whiteToPlay == true ? ChessCore.Piece.White : ChessCore.Piece.Black;
        }

        public List<Move> GetValidMoveFrom(int position)
        {
            List<int> moves = finder.FindAllMovesFromPosition(position);
            List<Move> movesObject = new List<Move>();
            foreach (int move in moves) 
            {
                movesObject.Add( new Move(move));
            }
            return movesObject;
        }

        public Result GetGameResult()
        {
            return result;
        }

        public void PlayPlayerMove(Move move)
        {
            PlayMove(move.GetMove());
        }

        private void PlayMove(int move)
        {
            int playedMove = MoveHelper.ExecuteMove(state, move);

            if (finder.FindAllMoves().Count == 0)
            {
                state.whiteToPlay = !state.whiteToPlay;
                if (ValidState.IsStateValid(state))
                {
                    result = Result.Draw;
                }
                else
                {
                    result = state.whiteToPlay ? Result.WhiteWin : Result.BlackWin;
                }
            }
        }

        public Move PlayComputerMove()
        {
            int move = AIPlayer.GetBestMove(state);
            PlayMove(move);
            return new Move(move);
        }
    }
}
