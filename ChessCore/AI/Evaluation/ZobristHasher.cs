using ChessCore.GameContext;
using ChessCore.Moves;
using System.Security.Cryptography;

namespace ChessCore.AI.Evaluation
{
    public class ZobristHasher
    {
        static private readonly int numberOfPieces = 12; // TODO: add Castling and en-passant to it
        static private readonly int boardSize = 64;


        private long[,] zobristTable = new long[numberOfPieces, boardSize];

        public void Initialize()
        {
            for (int i = 0; i < numberOfPieces; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    zobristTable[i, j] = GenerateBitString();
                }
            }
        }

        public long Hash(int[] board)
        {
            long hash = 0;
            for (int index = 0; index < boardSize; index++)
            {
                int piece = board[index];
                if (piece == 0) continue;

                hash = AddPieceToHash(hash, piece, index);
            };

            return hash;
        }

        public long UpdateHash(int[] board, bool whiteToPlay, long previousHash, int executedMove)
        {
            int startPos = MoveHelper.GetStartPos(executedMove);
            int endPos = MoveHelper.GetEndPos(executedMove);

            // TODO: Handle all aspect of state(castle / en passant available, whose turn it is, etc...)

            int moveType = MoveHelper.GetMoveType(executedMove);
            if (moveType == MoveHelper.SimpleMove)
            {
                int movedPiece = board[endPos];
                int eatenPiece = MoveHelper.GetEateanPiece(executedMove);

                if (eatenPiece != 0) previousHash = AddPieceToHash(previousHash, eatenPiece, endPos); // Remove hash from eaten piece
                previousHash = AddPieceToHash(previousHash, movedPiece, endPos); // Add hash of the moved piece in the end position

                previousHash = AddPieceToHash(previousHash, movedPiece, startPos); // Remove hash from the moved piece from the start position
            }

            else if (moveType == MoveHelper.Promotion)
            {
                int eatenPiece = MoveHelper.GetEateanPiece(executedMove);
                int movedPiece = Piece.Pawn | (whiteToPlay ? Piece.Black : Piece.White);
                int promotedPiece = board[endPos];

                if (eatenPiece != 0) previousHash = AddPieceToHash(previousHash, eatenPiece, endPos); // Remove hash from eaten piece
                previousHash = AddPieceToHash(previousHash, promotedPiece, endPos); // Add hash of the promoted piece in the end position

                previousHash = AddPieceToHash(previousHash, movedPiece, startPos); // Remove hash from the moved pawn from the start position
            }

            else if (moveType == MoveHelper.Castlebl)
            {
                int rookPiece = Piece.Rook & Piece.Black;
                int kingPiece = Piece.King & Piece.Black;

                previousHash = AddPieceToHash(previousHash, rookPiece, 0); // Removed Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, 4); // Removed King
                previousHash = AddPieceToHash(previousHash, rookPiece, 3); // Added Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, 2); // Added King
            }

            else if (moveType == MoveHelper.Castlebr)
            {
                int rookPiece = Piece.Rook & Piece.Black;
                int kingPiece = Piece.King & Piece.Black;

                previousHash = AddPieceToHash(previousHash, rookPiece, 7); // Removed Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, 4); // Removed King
                previousHash = AddPieceToHash(previousHash, rookPiece, 5); // Added Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, 6); // Added King
            }

            else if (moveType == MoveHelper.Castlewl)
            {
                int rookPiece = Piece.Rook & Piece.White;
                int kingPiece = Piece.King & Piece.White;

                previousHash = AddPieceToHash(previousHash, rookPiece, 56); // Removed Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, 60); // Removed King
                previousHash = AddPieceToHash(previousHash, rookPiece, 59); // Added Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, 58); // Added King
            }

            else if (moveType == MoveHelper.Castlewr)
            {
                int rookPiece = Piece.Rook & Piece.White;
                int kingPiece = Piece.King & Piece.White;

                previousHash = AddPieceToHash(previousHash, rookPiece, 64); // Removed Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, 60); // Removed King
                previousHash = AddPieceToHash(previousHash, rookPiece, 61); // Added Rook
                previousHash = AddPieceToHash(previousHash, kingPiece, 62); // Added King
            }

            else if (moveType == MoveHelper.EnPassant)
            {
                int eatenPiece = MoveHelper.GetEateanPiece(executedMove);
                int movedPiece = Piece.Pawn | (whiteToPlay ? Piece.Black : Piece.White);

                int eatenPiecePos;

                eatenPiecePos = startPos + ((endPos - startPos == -9 || endPos - startPos == 7) ? 1 : -1);

                previousHash = AddPieceToHash(previousHash, eatenPiece, eatenPiecePos); // Eaten Pawn
                previousHash = AddPieceToHash(previousHash, movedPiece, startPos); // Removed Pawn
                previousHash = AddPieceToHash(previousHash, movedPiece, endPos); // Added Pawn
            }

            return previousHash;
        }

        private int GetPieceIndex(int piece)
        {
            int index = piece - ((piece | Piece.ColorFilter) + 1);
            if (index < 0 || piece >= numberOfPieces) throw new Exception("Invalid Piece Index");
            return index;
        }

        private long GenerateBitString()
        {
            byte[] bytes = new byte[8];
            RandomNumberGenerator.Fill(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        private long AddPieceToHash(long hash, int piece, int position)
        {
            int pieceIndex = GetPieceIndex(piece);
            hash ^= zobristTable[pieceIndex, position];
            return hash;
        }
    }
}
