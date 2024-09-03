
using ChessCore.FindValidMoves;
using ChessCore.GameContext;

namespace ChessCore.Moves
{
    public static class MoveHelper
    {
        // Flags
        // 0-3: Type, 4-9: EndPos, 10-15: StartPos, 16-21 Tag, 28-31: Last EnPassant
        // Tag 0-11
        // 0-2: White Castle, 3-5: Black Castle, 6-9 EnPassant
        private const int MoveTypeFilter = 0xF;

        private const int EndPosFilter = 0x3f0;
        private const int StartPosFilter = 0xFC00;

        private const int TagFilter = 0x003F0000;


        public static int CreateMove(int startPos, int endPos, int type = 1, int args = 0) 
        {
            return type | (endPos << 4) | (startPos << 10) | args << 16;
        }

        public static bool CreateValidState(State state, int move)
        {
            move = ExecuteMove(state, move);
            var result = ValidState.IsStateValid(state);
            RevertMove(state, move);
            return result;
        }

        public static int ExecuteMove(State state, int move)
        {
            state.whiteToPlay = !state.whiteToPlay;
            var endPosition = GetEndPos(move);
            var startPosition = GetStartPos(move);

            var eatenPiece = 0;

            move = SetOldEnPassant(move, (state.GetFlags() & (0xf << 6))>>6);
            state.RemoveFlags(((1 << 14) - 1) << 6);

            switch (GetMoveType(move))
            {
                case (int)MoveType.SimpleMove:
                    eatenPiece = state.board[endPosition];
                    state.board[endPosition] = state.board[startPosition];
                    state.board[startPosition] = 0;

                    state.AddFlags(GetFlags(move));
                    break;

                case (int)MoveType.EnPassant:
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

                case (int)MoveType.Promotion:
                    var promotedInto = (endPosition/8 == 0 ? Piece.White : Piece.Black) | GetFlags(move);
                    eatenPiece = state.board[endPosition];
                    state.board[endPosition] = promotedInto;
                    state.board[startPosition] = 0; 
                    break;

                case (int)MoveType.Castlewl:
                case (int)MoveType.Castlewr:
                case (int)MoveType.Castlebl:
                case (int)MoveType.Castlebr:
                    var castleMove = CastleMove.GetCastleMove(GetMoveType(move));
                    
                    state.board[castleMove.StartKingPos] = 0;
                    state.board[castleMove.EndRookPos] = castleMove.Color | Piece.King;
                    state.board[castleMove.EndKingPos] = castleMove.Color | Piece.Rook;
                    state.board[castleMove.StartRookPos] = 0;
                    
                    state.AddFlags(State.kingMove << castleMove.Color == Piece.White ? 0 : 3);
                    break;
            }

            move = SetEatenPiece(move, eatenPiece);
            return move;
        }

        public static void RevertMove(State state, int move)
        {
            state.whiteToPlay = !state.whiteToPlay;
            var endPosition = GetEndPos(move);
            var startPosition = GetStartPos(move);

            var eatenPiece = GetEateanPiece(move);

            switch(GetMoveType(move))
            {
                case (int)MoveType.SimpleMove:
                    state.board[startPosition] = state.board[endPosition];
                    state.board[endPosition] = eatenPiece;
                    state.RemoveFlags(GetFlags(move));
                    break;

                case (int)MoveType.EnPassant:
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

                case (int)MoveType.Promotion:
                    var promotedFrom = (endPosition / 8 == 0 ? Piece.White : Piece.Black) | Piece.Pawn;
                    state.board[startPosition] = promotedFrom;
                    state.board[endPosition] = eatenPiece;
                    break;

                case (int)MoveType.Castlewl:
                case (int)MoveType.Castlewr:
                case (int)MoveType.Castlebl:
                case (int)MoveType.Castlebr:
                    var castleMove = CastleMove.GetCastleMove(GetMoveType(move));
                    
                    state.board[castleMove.StartKingPos] = castleMove.Color | Piece.King;
                    state.board[castleMove.EndRookPos] = 0;
                    state.board[castleMove.EndKingPos] = 0;
                    state.board[castleMove.StartRookPos] = castleMove.Color | Piece.Rook;
                    
                    state.AddFlags(State.kingMove << castleMove.Color == Piece.White ? 0 : 3);
                    break;
            }
            state.AddFlags((GetEnPassant(move) << 6));
        }

        public static int GetStartPos(int move) 
        {
            return (move & StartPosFilter) >> 10;
        }

        public static int GetEndPos(int move)
        {
            return (move & EndPosFilter) >> 4;
        }

        public static int GetMoveType(int move)
        {
            return (move & MoveTypeFilter);
        }

        private static int GetFlags(int move)
        {
            var tags = (move & TagFilter) >> 16;
            var type = tags & 3;
            switch (type)
            {
                case 0: //No Tags
                    return 0;
                case 1: //Castle
                    var shift = ((tags >> 2) & 0x1) == 1 ? 0 : 3;
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

        public static int SetEatenPiece(int move, int eatenPiece)
        {
            move &= ~(0x1F << 23);
            move |= (eatenPiece << 23);
            return move;
        }

        public static int GetEateanPiece(int move)
        {
            return (move >> 23) & 0x1F;
        }

        public static int SetOldEnPassant(int move, int enPassant)
        {
            move &= 0x0FFFFFFF;
            move |= (enPassant << 28);
            return move;
        }

        public static int GetEnPassant(int move)
        {
            return (move >> 28) & 0xF;
        }

        public static int GetPlayerColor(State state, int move)
        {
            var startPosition = GetStartPos(move);
            var piece = state.board[startPosition];

            if (piece == 0) return 0;

            return (piece & Piece.White) != 0 ? Piece.White : Piece.Black;
        }
    }
}
