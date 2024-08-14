namespace ChessCore.GameContext
{
    public class State
    {
        public bool whiteToPlay;

        public int[] board = new int[64];

        private int flags = 0;

        public const int canCastleLeft = 3;
        public const int canCastleRight = 6;

        internal const int lRookMove = 1;
        internal const int kingMove = 2;
        internal const int rRookMove = 4;

        public State()
        {
            CreateStateFromFenString("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -");
        }

        public State(string fenString)
        {
            CreateStateFromFenString(fenString);
        }

        public State(State copiedState)
        {
            whiteToPlay = copiedState.whiteToPlay;
            flags = copiedState.flags;

            Array.Copy(copiedState.board, board, 64);
        }

        public void AddFlags(int flags)
        {
            this.flags |= flags;
        }

        public void RemoveFlags(int flags)
        {
            this.flags &= ~flags;
        }

        public int GetFlags()
        {
            return flags;
        }

        private void CreateStateFromFenString(string fenString)
        {
            string[] fenParts = fenString.Split(' ');

            // Part 1
            string[] rows = fenParts[0].Split('/');

            int index = 0;
            foreach (string row in rows)
            {
                foreach (char c in row)
                {
                    if (char.IsDigit(c))
                    {
                        index += int.Parse(c.ToString());
                    }
                    else
                    {
                        board[index++] = GetPieceFromFenChar(c);
                    }
                }
            }

            // Part 2
            whiteToPlay = fenParts[1] == "w";

            // Part 3
            // TO DO

            // Part 4
            // TO DO
        }

        private int GetPieceFromFenChar(char fenChar)
        {
            switch (fenChar)
            {
                case 'p': return Piece.Black | Piece.Pawn;
                case 'n': return Piece.Black | Piece.Knight;
                case 'b': return Piece.Black | Piece.Bishop;
                case 'r': return Piece.Black | Piece.Rook;
                case 'q': return Piece.Black | Piece.Queen;
                case 'k': return Piece.Black | Piece.King;
                case 'P': return Piece.White | Piece.Pawn;
                case 'N': return Piece.White | Piece.Knight;
                case 'B': return Piece.White | Piece.Bishop;
                case 'R': return Piece.White | Piece.Rook;
                case 'Q': return Piece.White | Piece.Queen;
                case 'K': return Piece.White | Piece.King;
                default:
                    throw new ArgumentException($"Invalid FEN character: {fenChar}");
            }
        }
    }
}
