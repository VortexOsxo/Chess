using ChessCore.AI.Evaluation;
using ChessCore.AI.Search;
using ChessCore.FindValidMoves;
using ChessCore.GameContext;
using ChessCore.Moves;

namespace ChessCore.AI
{
    public class AIPlayer : Player
    {
        public override void OnPlayerTurn()
        {
            int bestMove = GetBestMove(game.GetState());

            game.PlayMove(bestMove);
        }

        static public int GetBestMove(State state)
        {
            return GetBestMoveScore(state).move;
        }

        static private MoveScore GetBestMoveScore(State state, int alpha = int.MinValue, int beta = int.MaxValue, int depth = 5)
        {
            List<int> moves = ValidMovesFinder.FindAllMoves(state);
            moves = MoveOrder.OrderMoves(state, moves);

            int bestMove = 0;
            int bestScore = state.whiteToPlay ? int.MinValue : int.MaxValue;

            int score;
            int move;
            foreach (int m in moves)
            {
                move = MoveHelper.ExecuteMove(state, m);
                if (depth == 1)
                {
                    score = Evaluator.EvaluatePosition(state);
                }
                else
                {
                    score = GetBestMoveScore(state, alpha, beta, depth - 1).score;
                }
                MoveHelper.RevertMove(state, move);

                if (state.whiteToPlay)
                {
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = m;
                    }
                    alpha = Math.Max(alpha, score);

                }
                else
                {
                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestMove = m;
                    }
                    beta = Math.Min(beta, score);
                }
                if (alpha >= beta) break;
            }
            return new MoveScore(bestMove, bestScore);
        }

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
    }
}