using ChessCommunication;
using ChessCore.AI;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        
        public override void OnGameEnded(Result result) { }

        public override void OnMovePlayed(int move)
        {
            handler.SendMessage((int) Messages.OnMovePlayed, move);
        }

        public override void OnPlayerTurn()
        {

        }

        private void OnMessageReceived(int code, int value)
        {
            if (code == (int) Messages.JoinGame)
            {
                var game = new Game(this, new AIPlayer());

                handler.SendMessage((int) Messages.OnGameJoined, color);
                game.Start();
            }

            if (code == (int) Messages.PlayMove)
            {
                game.PlayMove(value);
            }
        }
    }
}
