
namespace ChessCore.Moves
{
    public class Move
    {
        private int move;

        public Move(int move)
        {
            this.move = move;
        }

        public int GetEndPosition()
        {
            return MoveHelper.GetEndPos(move);
        }

        public bool IsPromotion()
        {
            return MoveHelper.GetMoveType(move) == MoveHelper.Promotion;
        }

        public void ChangePromotionType(int type)
        {
            if (!IsPromotion()) return;
            move &= 0xFFFF;
            move |= type << 16;
        }

        public int GetMove()
        {
            return move;
        }
    }
}
