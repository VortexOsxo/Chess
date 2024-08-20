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
        
        public override void OnGameEnded(Result result) { }

        public override void OnMovePlayed(int move)
        {
            handler.SendMessage(Encoder.EncodeMove(move));
        }

        public override void OnPlayerTurn()
        {

        }

        private void OnMessageReceived(byte[] data)
        {
            // Game Managing
            if (data[0] == 1)
            {
                if (data[1] == 0)
                {
                    var game = new Game(this, new AIPlayer());

                    handler.SendMessage([1, 1, (byte)color]);

                    game.Start();
                }
            }
            // In Game Action
            if (data[0] == 2)
            {
                // Playing a move
                if (data[1] == 1)
                {
                    int move = Encoder.DecodeMove(data);
                    game.PlayMove(move);
                }
            }
        }
    }
}
