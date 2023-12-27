
using ChessCore.FindValidMoves;

namespace ChessCore.Moves
{
    public class MoveHelper
    {
        public const int SimpleMove = 1;
        public const int EnPassant = 2;
        
        public const int Castlewl = 3;
        public const int Castlewr = 4;
        public const int Castlebl = 5;
        public const int Castlebr = 6;

        public const int Promotion = 7;


        // Flags
        // 0-3: Type, 4-9: EndPos, 10-15: StartPos, 16-21 Tag, 28-31: Last EnPassant
        // Tag 0-11
        // 0-2: White Castle, 3-5: Black Castle, 6-9 EnPassant
        private const int MoveTypeFilter = 0xF;

        private const int EndPosFilter = 0x3f0;
        private const int StartPosFilter = 0xFC00;

        private const int TagFilter = 0x003F0000;

        static int eatenPiece = 0;


        static public int CreateMove(int startPos, int endPos, int type = 1, int args = 0) 
        {
            return type | (endPos << 4) | (startPos << 10) | args << 16;
        }

        static public bool CreateValidState(State state, int move)
        {
            bool result;
            move = ExecuteMove(state, move);
            result = ValidState.IsStateValid(state);
            RevertMove(state, move);
            return result;
        }

        static public int ExecuteMove(State state, int move)
        {
            int endPosition = GetEndPos(move);
            int startPosition = GetStartPos(move);

            move = SetOldEnPassant(move, (state.GetFlags() & (0xf << 6))>>6);
            state.RemoveFlags(((1 << 14) - 1) << 6);

            switch (GetMoveType(move))
            {
                case SimpleMove:
                    eatenPiece = state.board[endPosition];
                    state.board[endPosition] = state.board[startPosition];
                    state.board[startPosition] = 0;

                    state.AddFlags(GetFlags(move));
                    break;

                case EnPassant:
                    if (endPosition - startPosition == -9
                        || endPosition - startPosition == 7)
                    {
                        eatenPiece = state.board[startPosition - 1];
                        state.board[startPosition - 1] = 0;
                    } else {
                        eatenPiece = state.board[startPosition + 1];
                        state.board[startPosition + 1] = 0;
                    }
                    state.board[endPosition] = state.board[startPosition];
                    state.board[startPosition] = 0;

                    state.AddFlags(GetFlags(move));
                    break;

                case Promotion:
                    int promotedInto = (endPosition/8 == 0 ? Piece.White : Piece.Black) | GetFlags(move);
                    eatenPiece = state.board[endPosition];
                    state.board[endPosition] = promotedInto;
                    state.board[startPosition] = 0;

                    break;

                case Castlewl:
                    state.board[56] = 0;
                    state.board[58] = Piece.White | Piece.King;
                    state.board[59] = Piece.White | Piece.Rook;
                    state.board[60] = 0;
                    
                    state.AddFlags(State.kingMove);
                    eatenPiece = 0;
                    break;

                case Castlewr:
                    state.board[63] = 0;
                    state.board[62] = Piece.White | Piece.King;
                    state.board[61] = Piece.White | Piece.Rook;
                    state.board[60] = 0;

                    state.AddFlags(State.kingMove);
                    eatenPiece = 0;
                    break;

                case Castlebl:
                    state.board[0] = 0;
                    state.board[2] = Piece.Black | Piece.King;
                    state.board[3] = Piece.Black | Piece.Rook;
                    state.board[4] = 0;

                    state.AddFlags(State.kingMove << 3);
                    eatenPiece = 0;
                    break;

                case Castlebr:
                    state.board[7] = 0;
                    state.board[6] = Piece.Black | Piece.King;
                    state.board[5] = Piece.Black | Piece.Rook;
                    state.board[4] = 0;

                    state.AddFlags(State.kingMove << 3);
                    eatenPiece = 0;
                    break;

                default:
                    break;
            }
            return move;
        }

        static public void RevertMove(State state, int move)
        {
            int endPosition = GetEndPos(move);
            int startPosition = GetStartPos(move);

            switch(GetMoveType(move))
            {
                case SimpleMove:
                    state.board[startPosition] = state.board[endPosition];
                    state.board[endPosition] = eatenPiece;
                    state.RemoveFlags(GetFlags(move));
                    break;

                case EnPassant:
                    if (endPosition - startPosition == -9
                        || endPosition - startPosition == 7)
                    {
                        state.board[startPosition - 1] = eatenPiece;
                    } else {
                        state.board[startPosition + 1] = eatenPiece;
                    }
                    state.board[startPosition] = state.board[endPosition];
                    state.board[endPosition] = 0;
                    state.RemoveFlags(GetFlags(move));
                    break;

                case Promotion:
                    int promotedFrom = (endPosition / 8 == 0 ? Piece.White : Piece.Black) | Piece.Pawn;
                    state.board[startPosition] = promotedFrom;
                    state.board[endPosition] = eatenPiece;
                    break;

                case Castlewl:
                    state.board[56] = Piece.White | Piece.Rook;
                    state.board[58] = 0;
                    state.board[59] = 0;
                    state.board[60] = Piece.White | Piece.King;
                    state.RemoveFlags(State.kingMove);
                    break;

                case Castlewr:
                    state.board[63] = Piece.White | Piece.Rook;
                    state.board[62] = 0;
                    state.board[61] = 0;
                    state.board[60] = Piece.White | Piece.King;
                    state.RemoveFlags(State.kingMove);
                    break;

                case Castlebl:
                    state.board[0] = Piece.Black | Piece.Rook;
                    state.board[2] = 0;
                    state.board[3] = 0;
                    state.board[4] = Piece.Black | Piece.King;
                    state.RemoveFlags(State.kingMove << 3);
                    break;

                case Castlebr:
                    state.board[7] = Piece.Black | Piece.Rook;
                    state.board[6] = 0;
                    state.board[5] = 0;
                    state.board[4] = Piece.Black | Piece.King;
                    state.RemoveFlags(State.kingMove << 3);
                    break;

                default:
                    break;
            }
            state.AddFlags((GetEnPassant(move) << 6));
        }

        static private int GetStartPos(int move) 
        {
            return (move & StartPosFilter) >> 10;
        }

        static public int GetEndPos(int move)
        {
            return (move & EndPosFilter) >> 4;
        }

        static public int GetMoveType(int move)
        {
            return (move & MoveTypeFilter);
        }

        static private int GetFlags(int move)
        {
            int tags = (move & TagFilter) >> 16;
            int type = tags & 3;
            switch (type)
            {
                case 0: //No Tags
                    return 0;
                case 1: //Castle
                    int shift = ((tags >> 2) & 0x1) == 1 ? 0 : 3;
                    switch ((tags>>3) & 3)
                    {
                        case 1:
                            return (1 << shift);
                        case 2:
                            return (2 << shift);
                        case 3:
                            return (4 << shift);
                    }
                    return 1;
                case 2: // En Passant
                    return (tags >> 2) << 6; // Position = tags >> 2 and then << 6 to give space for castle
                case 3: // Promotion
                    return (tags >> 2);

                default:
                    return 0;
            }
        }

        static public int SetEatenPiece(int move, int eatenPiece) // We should still have enough space to add the color directly in there
        {
            //move &= 0xF0FFFFFF;
            move |= (eatenPiece << 24);
            return move;
        }

        static public int GetEateanPiece(int move)
        {
            return (move << 24) & 0x0F;
        }

        static public int SetOldEnPassant(int move, int enPassant)
        {
            move &= 0x0FFFFFFF;
            move |= (enPassant << 28);
            return move;
        }

        static public int GetEnPassant(int move)
        {
            return (move >> 28) & 0xf;
        }

    }
}
