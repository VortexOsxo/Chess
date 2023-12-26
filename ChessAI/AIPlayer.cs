using ChessCore;
using ChessCore.FindValidMoves;
using ChessCore.Moves;

namespace ChessAI
{
    public class AIPlayer
    {

        private class MoveScore
        {
            public int move;
            public int score;
            public MoveScore(int move, int score)
            {
                this.move = move;
                this.score = score;
            }
        }

        static public int GetBestMove(State state)
        {
            return GetBestMoveScore(state).move;
        }
    
        static private MoveScore GetBestMoveScore(State state)
        {
            List<int> moves = new ValidMovesFinder(state).FindAllMoves();

            int bestMove = 0;
            int bestScore = state.whiteToPlay ? int.MinValue : int.MaxValue;

            int score;
            int move;
            foreach (int m in moves)
            {
                move = MoveHelper.ExecuteMove(state, m);

                //List<int> a = new ValidMovesFinder(state).FindAllMoves();

                score = Evaluation.Evaluator.EvaluatePosition(state);
                if (state.whiteToPlay ? score > bestScore : score < bestScore)
                {
                    bestScore = score;
                    bestMove = m;
                }
                MoveHelper.RevertMove(state, move);
            }
            return new MoveScore(bestMove, bestScore);
        }
    }
}