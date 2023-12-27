
namespace ChessCore.FindValidMoves
{
    public class ValidState
    {
        static public bool IsStateValid(State state)
        {
            int colorChecked = state.whiteToPlay ? Piece.Black : Piece.White;
            int kingPosition = -1;
            for (int i = 0; i < 64; i++)
            {
                if ((state.board[i] & Piece.PieceFilter) == Piece.King
                    && (state.board[i] & Piece.ColorFilter) == colorChecked)
                {
                    kingPosition = i;
                    break;
                }
            }
            if (kingPosition == -1) return false;
            return !IsPositionAttackedBy(state, state.whiteToPlay ? Piece.White : Piece.Black, kingPosition);
        }

        static public bool IsPositionAttackedBy(State state, int colorChecked, int kingPosition) 
        {
            int newPosition;
            //Bishop and Queen
            foreach (int direction in new int[] { -9, -7, 7, 9 })
            {
                newPosition = kingPosition;

                while (BoardHelper.IsNotGettingOutOfTheBoard(newPosition, direction))
                {
                    newPosition += direction;
                    if (state.board[newPosition] == 0)
                    {
                        continue;
                    }
                    if ((state.board[newPosition] & Piece.ColorFilter) == colorChecked &&
                       ((state.board[newPosition] & Piece.PieceFilter) == Piece.Queen
                       || (state.board[newPosition] & Piece.PieceFilter) == Piece.Bishop))
                    {
                        return true;
                    }
                    break;
                }
            }

            //Rook & Queen
            foreach (int direction in new int[] { -8, -1, 1, 8 })
            {
                newPosition = kingPosition;

                while (BoardHelper.IsNotGettingOutOfTheBoard(newPosition, direction))
                {
                    newPosition += direction;
                    if (state.board[newPosition] == 0)
                    {
                        continue;
                    }
                    if ((state.board[newPosition] & Piece.ColorFilter) == colorChecked &&
                       ((state.board[newPosition] & Piece.PieceFilter) == Piece.Queen
                       || (state.board[newPosition] & Piece.PieceFilter) == Piece.Rook))
                    {
                        return true;
                    }
                    break;
                }
            }

            //Knight
            foreach (int direction in new int[] { -17, -15, -10, -6, 6, 10, 15, 17 })
            {
                newPosition = kingPosition;

                if (BoardHelper.IsNotGettingOutOfTheBoard(newPosition, direction))
                {
                    newPosition += direction;
                    if ((state.board[newPosition] & Piece.ColorFilter) == colorChecked &&
                        ((state.board[newPosition] & Piece.PieceFilter) == Piece.Knight))
                    {
                        return true;
                    }

                }
            }

            //King
            foreach (int direction in new int[] { -9, -7, 7, 9, -8, -1, 1, 8 })
            {
                newPosition = kingPosition;

                if (BoardHelper.IsNotGettingOutOfTheBoard(newPosition, direction))
                {
                    newPosition += direction;
                    if ((state.board[newPosition] & Piece.ColorFilter) == colorChecked &&
                        ((state.board[newPosition] & Piece.PieceFilter) == Piece.King))
                    {
                        return true;
                    }

                }
            }

            //Pawn
            int[] pawnDirections = colorChecked == Piece.Black ? new int[] { -7, -9 } : new int[] { 7, 9 };
            foreach (int direction in pawnDirections)
            {
                newPosition = kingPosition;

                if (BoardHelper.IsNotGettingOutOfTheBoard(newPosition, direction))
                {
                    newPosition += direction;
                    if ((state.board[newPosition] & Piece.ColorFilter) == colorChecked &&
                        ((state.board[newPosition] & Piece.PieceFilter) == Piece.Pawn))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
