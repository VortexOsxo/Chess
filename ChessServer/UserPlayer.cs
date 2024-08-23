using ChessCommunication;
using ChessCore.AI;

namespace ChessCore.GameContext
{
    internal class UserPlayer : Player
    {
        private SocketHandler handler;

        public UserPlayer(SocketHandler handlerIn)
        {
            handler = handlerIn;
            handler.onMessageReceived = OnMessageReceived;
        }

        public override void OnGameStarted(Game game, int color)
        {
            base.OnGameStarted(game, color);
            handler.SendMessage((int)Messages.OnGameJoined, color);
        }

        public override void OnGameEnded(Result result) { }

        public override void OnMovePlayed(int move, bool playedBySelf = false)
        {
            Messages message = playedBySelf ? Messages.OnSelfMovePlayed : Messages.OnEnemyMovePlayed;
            handler.SendMessage((int) message, move);
        }

        public override void OnPlayerTurn()
        {

        }

        private void OnMessageReceived(int code, int value)
        {
            if (code == (int) Messages.JoinSinglePlayerGame)
            {
                var instance = GameManager.Instance;
                instance.JoinSinglePlayerGame(this);
            }

            else if (code == (int) Messages.JoinMultiPlayerGame)
            {
                var instance = GameManager.Instance;
                instance.JoinGameQueue(this);
            }

            else if (code == (int)Messages.LeaveQueue)
            {
                var instance = GameManager.Instance;
                instance.LeaveGameQueue(this);
            }

            else if (code == (int) Messages.PlayMove)
            {
                game.PlayMove(value);
            }
        }
    }
}
