using ChessCore;
using ChessCore.Moves;

namespace ChessAI.Evaluation
{
    internal class Evaluator
    {
        //Positive result means an advantage for white and a negative result means an advantage for black
        static public int EvaluatePosition(State state)
        {
            int score = 0;
            for (int i = 0; i < 64; ++i)
            {
                if (state.board[i] == 0) continue;
                score += Material.EvaluatePieceValue(state, i);
            }
            return score;
        }  
    }
}
