using ChessCore;

namespace ChessAI.Evaluation
{
    internal class Material
    {
        public static int EvaluatePieceValue(State state, int position)
        {
            // If its black to play, we need to flip the position horizontally (the column stays the same but we change the row)
            int lookUpPosition = state.whiteToPlay ? position : (7 - (position / 8)) * 8 + (position % 8);
            int piece = state.board[position] & Piece.PieceFilter;
            if (piece==0) return 0;
            int score = piecesValue[piece];
            switch (piece)
            {
                case Piece.Pawn:
                    score += pawnValuesByPosition[lookUpPosition];
                    break;
                case Piece.Knight:
                    score += knightValuesByPosition[lookUpPosition];
                    break;
                case Piece.Bishop:
                    score += bishopValuesByPosition[lookUpPosition];
                    break;
                case Piece.Rook:
                    score += rookValuesByPosition[lookUpPosition];
                    break;
                case Piece.Queen:
                    score += queenValuesByPosition[lookUpPosition];
                    break;
                case Piece.King:
                    score += kingValuesByPosition[lookUpPosition];
                    break;
            }
            return (state.board[position] & Piece.ColorFilter) == Piece.White ? score : -score;
        }

        static private Dictionary<int, int> piecesValue = new Dictionary<int, int>
        {
            { Piece.Pawn, 100 },
            { Piece.Bishop, 275 },
            { Piece.Knight, 300 },
            { Piece.Rook, 500 },
            { Piece.Queen, 900 },
            { Piece.King, 1000000 },
        };

        static private int[] pawnValuesByPosition = new int[64]
        {
            0, 0,  0,  0,  0,  0,  0,  0,
            50,50, 50, 50, 50, 50, 50, 50,
            10,10, 20, 30, 30, 20, 10, 10,
            5,  5, 10, 25, 25, 10,  5,  5,
            0,  0,  0, 20, 20,  0,  0,  0,
            5,  0, 5,  0,  0,   5,  0,  5,
            5, 10, 10,-30,-30, 10, 10,  5,
            5, 5,  5,   5,  5,  5,  5,  5
        };

        static private int[] knightValuesByPosition = new int[64]
        {
            -20,-10,-10,-10,-10,-10,-10,-20,
            -10, -5,  0,  0,  0,  0,-20,-10,
            -10,  0, 10, 15, 15, 10,  0,-10,
            -10,  5, 15, 20, 20, 15,  5,-10,
            -10,  5, 15, 20, 20, 15,  5,-10,
            -10,  0, 10, 15, 15, 10,  0,-10,
            -10, -5,  0,  5,  5,  0, -5,-10,
            -20,-10,-10,-10,-10,-10,-10,-20
        };

        static private int[] bishopValuesByPosition = new int[64]
        {
            -15,-10,-10,-10,-10,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5, 10, 10,  5,  0,-10,
            -10,  5,  5, 10, 10,  5,  5,-10,
            -10,  0, 10, 10, 10, 10,  0,-10,
            -10, 10, 10, 10, 10, 10, 10,-10,
            -10,  5,  0,  0,  0,  0,  5,-10,
            -15,-10,-10,-10,-10,-10,-10,-15
        };

        static private int[] rookValuesByPosition = new int[64]
        {
             0,  0,  0,  0,  0,  0,  0,  0,
            20, 20, 20, 20, 20, 20, 20, 20,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
             0,  0,  0,  0,  0,  0,  0,  0,
             0,  0,  0, 20, 10,  20,  0,  0
        };

        static private int[] queenValuesByPosition = new int[64]
        {
            -20,-10,-10, -5, -5,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5,  5,  5,  5,  0,-10,
             -5,  0,  5,  5,  5,  5,  0, -5,
              0,  0,  5,  5,  5,  5,  0, -5,
            -10,  5,  5,  5,  5,  5,  0,-10,
            -10,  0,  5,  0,  0,  0,  0,-10,
            -20,-10,-10,  5, -5,-10,-10,-20
        };

        static private int[] kingValuesByPosition = new int[64]
        {
            -30,-30,-30,-30,-30,-30,-30,-30,
            -30,-30,-30,-30,-30,-30,-30,-30,
            -30,-30,-30,-30,-30,-30,-30,-30,
            -20,-20,-20,-20,-20,-20,-20,-20,
            -15,-20,-20,-20,-20,-20,-20,-15,
            -10,-10,-10,-10,-10,-10,-10,-15,
              0,  5,  0,  0,  0,  0,  5,  0,
              0,  5, 50,  0,  0,  5, 50,  5
        };
    }
}
