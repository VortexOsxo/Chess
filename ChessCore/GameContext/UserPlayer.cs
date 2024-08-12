using ChessCore.Moves;

namespace ChessCore.GameContext
{
    public class UserPlayer : Player
    {
        public Game game;
        public int color;
        public Action? onPlayerTurnCallback;

        public void Play(Move move)
        {
            game.PlayMove(move.GetMove());
        }

        public override void OnGameStarted(Game game, int color)
        {
            this.game = game;
            this.color = color;
        }
        
        public override void OnGameEnded(Result result) { }


        public override void OnPlayerTurn()
        {
            onPlayerTurnCallback?.Invoke();
        }
    }
}
