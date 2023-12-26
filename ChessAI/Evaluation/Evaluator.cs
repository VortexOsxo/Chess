using ChessCore;

namespace ChessAI.Evaluation
{
    internal class Evaluator
    {

        //Positive result means an advantage for white and a negative result means an advantage for black
        static public int EvaluatePosition(State state)
        {
            int score = 0;
            for (int i = 0; i < 64; ++i)
            {
                if (state.board[i] == 0) continue;
                score += (state.board[i] & Piece.ColorFilter) == Piece.White
                    ? piecesValue[state.board[i] & Piece.PieceFilter]
                    : -piecesValue[state.board[i] & Piece.PieceFilter];
            }
            return score;
        }

        static private Dictionary<int, int> piecesValue = new Dictionary<int, int>
        {
            { Piece.Pawn, 1 },
            { Piece.Bishop, 3 },
            { Piece.Knight, 3 },
            { Piece.Rook, 5 },
            { Piece.Queen, 9 },
            { Piece.King, int.MaxValue },
        };

    }
}
