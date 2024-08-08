using ChessCore.Moves;
using ChessCore.AI.Evaluation;
using ChessCore.GameContext;

namespace ChessCore.AI.Search
{
    internal class MoveOrder
    {
        private class MoveComparator : IComparer<int>
        {
            static State state;

            public static void SetState(State state)
            {
                MoveComparator.state = state;
            }

            public int Compare(int move1, int move2)
            {
                return GetMoveScore(state, move1).CompareTo(GetMoveScore(state, move2));
            }
        }

        private static int GetMoveScore(State state, int move)
        {
            int startPos = MoveHelper.GetStartPos(move);
            int endPos = MoveHelper.GetEndPos(move);
            return 2 * Material.EvaluatePieceValue(state, endPos) - Material.EvaluatePieceValue(state, startPos);
        }

        public static List<int> OrderMoves(State state, List<int> moves)
        {
            MoveComparator.SetState(state);
            moves.Sort(new MoveComparator());
            return moves;
        }
    }
}
