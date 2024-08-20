using ChessCommunication;
using ChessCore.GameContext;
using ChessCore.Moves;
using ChessView.Views.GameView.ViewState;
using System.Net.Sockets;

namespace ChessView.Views.GameView
{
    internal class ClientPlayer
    {
        public State? state = null;
        public int color = 0;

        private SocketHandler client;
        private MainView mainView;

        public ClientPlayer(MainView mainViewIn)
        {
            mainView = mainViewIn;

            client = new SocketHandler(new TcpClient("127.0.0.1", 13000));
            client.onMessageReceived = OnMessageReceived;

            client.SendMessage(Encoder.JoinGame);
            
            state = new State();
        }

        public void OnMessageReceived(byte[] data)
        {
            // Game Managing
            if (data[0] == 1)
            {
                // Join Game Response
                if (data[1] == 1)
                {
                    state = new State();
                    color = data[2];
                    mainView.SetState(new Neutral());
                }
            }
            // In Game Action
            else if (data[0] == 2)
            {
                // Transmitting a played move
                if (data[1] == 1)
                {
                    int move = Encoder.DecodeMove(data);
                    MoveHelper.ExecuteMove(state, move);
                    mainView.SetState(new Neutral(new Move(move)));
                }
            }
        }

        public void Play(Move move)
        {
            var a = move.GetMove();
            client.SendMessage(Encoder.EncodeMove(move.GetMove()));
        }

        public void Close()
        {
            client.Close();
        }

    }
}
