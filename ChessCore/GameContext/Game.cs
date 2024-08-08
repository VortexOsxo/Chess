using ChessCore.AI;
using ChessCore.FindValidMoves;
using ChessCore.Moves;

namespace ChessCore.GameContext
{
    public class Game
    {
        private State state;
        private ValidMovesFinder finder;
        private Result result = Result.InProgress;
    
        public Game(string? fenString = null) 
        {
            state = fenString is null ? new State() : new State(fenString);
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
            return state.whiteToPlay == true ? Piece.White : Piece.Black;
        }

        public State GetState()
        {
            return state;
        }

        public List<Move> GetValidMoveFrom(int position)
        {
            List<int> moves = finder.FindAllMovesFromPosition(position);
            List<Move> movesObject = new List<Move>();
            foreach (int move in moves) 
            {
                movesObject.Add( new Move(move) );
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
            MoveHelper.ExecuteMove(state, move);

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
