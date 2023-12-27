using ChessCore.Moves;

namespace ChessCore.FindValidMoves
{
    public class ValidMovesFinder
    {
        private State state;

        public ValidMovesFinder(State state)
        {
            this.state = state;
        }

        public List<int> FindAllMoves(bool bVerifySelfCheck = true)
        {
            int colorToPlay = state.whiteToPlay ? Piece.White : Piece.Black;

            int piece;
            List<int> result = new List<int>();
            for (int i = 0; i < 64; ++i)
            {
                piece = state.board[i];
                if ((piece & Piece.ColorFilter) == colorToPlay)
                {
                    result.AddRange(FindAllMovesFromPosition(i, bVerifySelfCheck));
                }
            }
            return result;
        }

        public List<int> FindAllMovesFromPosition(int position, bool bVerifySelfCheck = true)
        {
            List<int> newMoves = new List<int>();

            int color, tags;
            int piece = state.board[position] & Piece.PieceFilter;
            switch (piece)
            {
                case Piece.Pawn:
                    color = state.board[position] & Piece.ColorFilter;

                    bool isFirstMove = position / 8 == (color == Piece.White ? 6 : 1);

                    int step = color == Piece.White ? -8 : 8;

                    if (BoardHelper.IsNotGettingOutOfTheBoard(position, step) && state.board[position + step] == 0)
                    {
                        if ((position + step) / 8 == 0 || (position + step) / 8 == 7)
                        {
                            newMoves.Add(MoveHelper.CreateMove(position, position + step, MoveHelper.Promotion, 3 | (Piece.Queen << 2)));
                        }

                        newMoves.Add(MoveHelper.CreateMove(position, position + step, 1));

                        int endPos = position + step * 2;
                        if (isFirstMove && state.board[endPos] == 0 && state.board[endPos] == 0)
                        {
                            newMoves.Add(MoveHelper.CreateMove(position, endPos, 1, 2 | (endPos - 24 << 2) )); // 10 for EnPassant type then the position
                        }  
                    }

                    if (BoardHelper.IsNotGettingOutOfTheBoard(position, step - 1) && (state.board[position + step - 1] & Piece.ColorFilter) != color
                        && (state.board[position + step - 1] != 0))
                        if ((position + step - 1) / 8 == 0 || (position + step - 1) / 8 == 7)
                            newMoves.Add(MoveHelper.CreateMove(position, position + step - 1, MoveHelper.Promotion, Piece.Queen));
                        else
                            newMoves.Add(MoveHelper.CreateMove(position, position + step - 1, 1));

                    if (BoardHelper.IsNotGettingOutOfTheBoard(position, step + 1) && (state.board[position + step + 1] & Piece.ColorFilter) != color
                        && (state.board[position + step + 1] != 0))
                        if ((position + step + 1) / 8 == 0 || (position + step + 1) / 8 == 7)
                            newMoves.Add(MoveHelper.CreateMove(position, position + step + 1, MoveHelper.Promotion, Piece.Queen));
                        else
                            newMoves.Add(MoveHelper.CreateMove(position, position + step + 1, 1));

                    // En Passant
                    if ((state.GetFlags() & (0xf << 6)) == 0)
                        break;

                    if (position - 1 == ((state.GetFlags() & 960) >> 6) + 24)
                        newMoves.Add(MoveHelper.CreateMove(position, position + step - 1, MoveHelper.EnPassant));

                    else if (position + 1 == ((state.GetFlags() & 960) >> 6) + 24)
                        newMoves.Add(MoveHelper.CreateMove(position, position + step + 1, MoveHelper.EnPassant));
                    break;

                case Piece.Knight:
                    newMoves = FindAllStepMovesFromPosition(knightSteps, position);
                    break;

                case Piece.Bishop:
                    newMoves = FindAllSlideMovesFromPosition(bishopSlides, position);
                    break;

                case Piece.Rook:
                    tags = 1 | ((state.board[position] & Piece.ColorFilter) == Piece.White ? (1 << 2) : 0); //01 because its a castle tag and 0-1 to indicate the color

                    int shift = (state.board[position] & Piece.ColorFilter) == Piece.White ? 0 : 3;
                    if (position % 8 == 0 && (state.GetFlags() & (State.lRookMove << shift)) == 0)
                    {
                        tags |= 1 << 3; // 1 for left rook
                    }
                    else if (position % 8 == 7 && (state.GetFlags() & (State.rRookMove << shift)) == 0)
                    {
                        tags |= 3 << 3; // 3 for right rook
                    }
                    else tags = 0; // No need to have a tag

                    newMoves = FindAllSlideMovesFromPosition(rookSlides, position, tags);
                    break;

                case Piece.Queen:
                    newMoves = FindAllSlideMovesFromPosition(queenSlides, position);
                    break;

                case Piece.King:
                    color = state.board[position] & Piece.ColorFilter;
                    if (color == Piece.White && (state.GetFlags() & State.kingMove) == 0)
                    {
                        tags = 1 | (1 << 2) | (2 << 3); // 01: Castle - 1: White - 10: KingMove
                    }
                    else if (color == Piece.Black && (state.GetFlags() & (State.kingMove << 3)) == 0)
                    {
                        tags = 1 | (0 << 2) | (2 << 3); // 01: Castle - 0: Black - 10: KingMove
                    }
                    else tags = 0;

                    newMoves = FindAllStepMovesFromPosition(kingStep, position, tags);

                    //  Castles            
                    if (color == Piece.White)
                    {
                        if ((state.GetFlags() & State.canCastleRight) == 0
                            && state.board[61] + state.board[62] == 0
                            && state.board[63] == (Piece.Rook | Piece.White)
                            && !ValidState.IsPositionAttackedBy(state, Piece.Black, 60)
                            && !ValidState.IsPositionAttackedBy(state, Piece.Black, 61)
                            && !ValidState.IsPositionAttackedBy(state, Piece.Black, 62))
                        {
                            newMoves.Add(MoveHelper.CreateMove(60, 62, MoveHelper.Castlewr));
                        }
                        if ((state.GetFlags() & State.canCastleLeft) == 0
                            && state.board[57] + state.board[58] + state.board[59] == 0
                            && state.board[56] == (Piece.Rook | Piece.White)
                            && !ValidState.IsPositionAttackedBy(state, Piece.Black, 57)
                            && !ValidState.IsPositionAttackedBy(state, Piece.Black, 58)
                            && !ValidState.IsPositionAttackedBy(state, Piece.Black, 59)
                            && !ValidState.IsPositionAttackedBy(state, Piece.Black, 60))
                        {
                            newMoves.Add(MoveHelper.CreateMove(60, 58, MoveHelper.Castlewl));
                        }
                        break;
                    }

                    if (((state.GetFlags() >> 3) & State.canCastleRight) == 0
                        && state.board[5] + state.board[6] == 0
                        && state.board[7] == (Piece.Rook | Piece.Black)
                        && state.board[7] == (Piece.Rook | Piece.Black)
                        && !ValidState.IsPositionAttackedBy(state, Piece.White, 4)
                        && !ValidState.IsPositionAttackedBy(state, Piece.White, 5)
                        && !ValidState.IsPositionAttackedBy(state, Piece.White, 6))
                    {
                        newMoves.Add(MoveHelper.CreateMove(4, 6, MoveHelper.Castlebr));
                    }
                    if (((state.GetFlags() >> 3) & State.canCastleLeft) == 0
                        && state.board[1] + state.board[2] + state.board[3] == 0
                        && state.board[0] == (Piece.Rook | Piece.Black)
                        && !ValidState.IsPositionAttackedBy(state, Piece.White, 1)
                        && !ValidState.IsPositionAttackedBy(state, Piece.White, 2)
                        && !ValidState.IsPositionAttackedBy(state, Piece.White, 3)
                        && !ValidState.IsPositionAttackedBy(state, Piece.White, 4))
                    {
                        newMoves.Add(MoveHelper.CreateMove(4, 2, MoveHelper.Castlebl));
                    }
                    break;
            }

            if(!bVerifySelfCheck) return newMoves;

            List<int> validMoves = new List<int>();
            foreach (int move in newMoves)
            {
                if (MoveHelper.CreateValidState(state, move))
                    validMoves.Add(move);
            }
            return validMoves;
        }

        private List<int> FindAllStepMovesFromPosition(int[] steps, int position, int tags = 0)
        {
            List<int> moves = new List<int>();
            int color = state.board[position] & Piece.ColorFilter;

            int slidingPosition;
            foreach (int direction in steps)
            {
                slidingPosition = position;

                if (BoardHelper.IsNotGettingOutOfTheBoard(slidingPosition, direction))
                {
                    slidingPosition += direction;
                    if ((state.board[slidingPosition] & Piece.ColorFilter) != color)
                        moves.Add(MoveHelper.CreateMove(position, slidingPosition, 1, tags));
                }
            }

            return moves;
        }

        private List<int> FindAllSlideMovesFromPosition(int[] slides, int position, int tags = 0)
        {
            List<int> moves = new List<int>();
            int color = state.board[position] & Piece.ColorFilter;

            int slidingPosition;
            foreach (int direction in slides)
            {
                slidingPosition = position;
                while (BoardHelper.IsNotGettingOutOfTheBoard(slidingPosition, direction))
                {
                    slidingPosition += direction;
                    if (state.board[slidingPosition] == 0)
                    {
                        moves.Add(MoveHelper.CreateMove(position, slidingPosition, 1, tags));
                        continue;
                    }
                    if ((state.board[slidingPosition] & Piece.ColorFilter) != color)
                    {
                        moves.Add(MoveHelper.CreateMove(position, slidingPosition, 1, tags));
                        break;
                    }
                    break;
                }
            }
            return moves;
        }

        static private int[] knightSteps = new int[] { -17, -15, -10, -6, 6, 10, 15, 17 };
        static private int[] bishopSlides = new int[] { -9, -7, 7, 9 };
        static private int[] rookSlides = new int[] { -8, -1, 1, 8 };
        static private int[] queenSlides = new int[] { -8, -1, 1, 8, -9, -7, 7, 9 };
        static private int[] kingStep = new int[] { -8, -1, 1, 8, -9, -7, 7, 9 };
    }
}
