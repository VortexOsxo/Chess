using ChessCommunication;
using ChessCore.GameContext;
using ChessCore.Moves;
using ChessView.Views.GameView.ViewState;

namespace ChessView.Views.GameView
{
    internal class ClientPlayer
    {
        public readonly State State;
        public readonly int Color = 0;

        private readonly SocketHandler client;
        private readonly MainView mainView;

        public ClientPlayer(MainView mainViewIn, int colorIn)
        {
            mainView = mainViewIn;

            client = ClientSocketHandler.Instance;
            client.onMessageReceived = OnMessageReceived;

            State = new State();
            Color = colorIn;
        }

        public void OnMessageReceived(int code, int value)
        {
            if (code == (int) Messages.OnSelfMovePlayed)
            {
                int move = value;
                MoveHelper.ExecuteMove(State, move);

                mainView.SetState(new ComputerTurn(mainView, this, new Move(move)));
            }

            else if (code == (int)Messages.OnEnemyMovePlayed)
            {
                int move = value;
                MoveHelper.ExecuteMove(State, move);

                mainView.SetState(new Neutral(mainView, this, new Move(move)));
            }
        }

        public void Play(Move move)
        {
            client.SendMessage((int) Messages.PlayMove, move.GetMove());
        }
    }
}
