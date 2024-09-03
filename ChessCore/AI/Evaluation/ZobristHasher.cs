using ChessCore.GameContext;
using ChessCore.Moves;
using System.Security.Cryptography;

namespace ChessCore.AI.Evaluation
{
    public class ZobristHasher
    {
        private const int NumberOfPieces = 12; // TODO: add Castling and en-passant to it
        private const int BoardSize = 64;


        private readonly long[,] zobristTable = new long[NumberOfPieces, BoardSize];

        public void Initialize()
        {
            for (var i = 0; i < NumberOfPieces; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    zobristTable[i, j] = GenerateBitString();
                }
            }
        }

        public long Hash(int[] board)
        {
            long hash = 0;
            for (var index = 0; index < BoardSize; index++)
            {
                var piece = board[index];
                if (piece == 0) continue;

                hash = AddPieceToHash(hash, piece, index);
            };

            return hash;
        }

        public long UpdateHash(int[] board, bool whiteToPlay, long previousHash, int executedMove)
        {
            var startPos = MoveHelper.GetStartPos(executedMove);
            var endPos = MoveHelper.GetEndPos(executedMove);

            // TODO: Handle all aspect of state(castle / en passant available, whose turn it is, etc...)

            var moveType = MoveHelper.GetMoveType(executedMove);
            if (moveType == (int)MoveType.SimpleMove)
            {
                var movedPiece = board[endPos];
                var eatenPiece = MoveHelper.GetEateanPiece(executedMove);

                if (eatenPiece != 0) previousHash = AddPieceToHash(previousHash, eatenPiece, endPos); // Remove hash from eaten piece
                previousHash = AddPieceToHash(previousHash, movedPiece, endPos); // Add hash of the moved piece in the end position

                previousHash = AddPieceToHash(previousHash, movedPiece, startPos); // Remove hash from the moved piece from the start position
            }

            else if (moveType == (int)MoveType.Promotion)
            {
                var eatenPiece = MoveHelper.GetEateanPiece(executedMove);
                var movedPiece = Piece.Pawn | (whiteToPlay ? Piece.Black : Piece.White);
                var promotedPiece = board[endPos];

                if (eatenPiece != 0) previousHash = AddPieceToHash(previousHash, eatenPiece, endPos); // Remove hash from eaten piece
                previousHash = AddPieceToHash(previousHash, promotedPiece, endPos); // Add hash of the promoted piece in the end position

                previousHash = AddPieceToHash(previousHash, movedPiece, startPos); // Remove hash from the moved pawn from the start position
            }

            else if (moveType is (int)MoveType.Castlebl or (int)MoveType.Castlebr or (int)MoveType.Castlewl or (int)MoveType.Castlewr)
            {
                var castleMove = CastleMove.GetCastleMove(moveType);
                
                var rookPiece = Piece.Rook | castleMove.Color;
                var kingPiece = Piece.King | castleMove.Color;

                previousHash = AddPieceToHash(previousHash, rookPiece, castleMove.StartRookPos); // Removed Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, castleMove.StartKingPos); // Removed King
                previousHash = AddPieceToHash(previousHash, rookPiece, castleMove.EndRookPos); // Added Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, castleMove.EndKingPos); // Added King
            }

            else if (moveType == (int)MoveType.EnPassant)
            {
                var eatenPiece = MoveHelper.GetEateanPiece(executedMove);
                var movedPiece = Piece.Pawn | (whiteToPlay ? Piece.Black : Piece.White);

                var eatenPiecePos = startPos + ((endPos - startPos == -9 || endPos - startPos == 7) ? 1 : -1);

                previousHash = AddPieceToHash(previousHash, eatenPiece, eatenPiecePos); // Eaten Pawn
                previousHash = AddPieceToHash(previousHash, movedPiece, startPos); // Removed Pawn
                previousHash = AddPieceToHash(previousHash, movedPiece, endPos); // Added Pawn
            }

            return previousHash;
        }

        private int GetPieceIndex(int piece)
        {
            var index = piece - ((piece | Piece.ColorFilter) + 1);
            if (index < 0 || piece >= NumberOfPieces) throw new Exception("Invalid Piece Index");
            return index;
        }

        private long GenerateBitString()
        {
            var bytes = new byte[8];
            RandomNumberGenerator.Fill(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        private long AddPieceToHash(long hash, int piece, int position)
        {
            var pieceIndex = GetPieceIndex(piece);
            hash ^= zobristTable[pieceIndex, position];
            return hash;
        }
    }
}
