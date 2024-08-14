using ChessCore.Moves;

namespace ChessCore.GameContext
{
    public class UserPlayer : Player
    {
        public Action? onPlayerTurnCallback;

        public void Play(Move move)
        {
            game.PlayMove(move.GetMove());
        }
        
        public override void OnGameEnded(Result result) { }


        public override void OnPlayerTurn()
        {
            onPlayerTurnCallback?.Invoke();
        }
    }
}
