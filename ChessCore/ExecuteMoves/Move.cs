
namespace ChessCore.Moves
{
    public class Move
    {
        private int move;

        public Move(int move)
        {
            this.move = move;
        }

        public int GetStartPosition()
        {
            return MoveHelper.GetStartPos(move);
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
            move |= (3 | (type << 2)) << 16;
        }

        public int GetMove()
        {
            return move;
        }
    }
}
