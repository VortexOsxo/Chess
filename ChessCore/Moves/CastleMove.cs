using System.Diagnostics;

namespace ChessCore.Moves;

public class CastleMove
{
    public readonly int StartKingPos;
    public readonly int EndKingPos;
    public readonly int StartRookPos;
    public readonly int EndRookPos;
    public readonly int Color;

    private CastleMove(int startKingPos, int endKingPos, int startRookPos, int endRookPos, int color)
    {
        StartKingPos = startKingPos;
        EndKingPos = endKingPos;
        StartRookPos = startRookPos;
        EndRookPos = endRookPos;
        Color = color;
    }

    public static CastleMove GetCastleMove(int castleCode)
    {
        switch (castleCode)
        {
            case (int)MoveType.Castlewl:
                return CastleWhiteLeft;
            case (int)MoveType.Castlewr:
                return CastleWhiteRight;
            case (int)MoveType.Castlebl:
                return CastleBlackLeft;
            case (int)MoveType.Castlebr:
                return CastleBlackRight;
        }
        Debug.Assert(false, "Invalid castle code");
        return null;
    }

    private static readonly CastleMove CastleWhiteLeft = new (60, 58, 56, 59, Piece.White);
    private static readonly CastleMove CastleWhiteRight = new (60, 62, 63, 61, Piece.White);
    private static readonly CastleMove CastleBlackLeft = new (4, 2, 0, 3, Piece.Black);
    private static readonly CastleMove CastleBlackRight = new (4, 6, 7, 5, Piece.Black);
    
}