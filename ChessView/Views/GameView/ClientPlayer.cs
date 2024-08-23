using ChessCommunication;
using ChessCore.GameContext;
using ChessCore.Moves;
using ChessView.Views.GameView.ViewState;

namespace ChessView.Views.GameView
{
    internal class ClientPlayer
    {
        public State? state = null;
        public int color = 0;

        private SocketHandler client;
        private MainView mainView;

        public ClientPlayer(MainView mainViewIn, int colorIn)
        {
            mainView = mainViewIn;

            client = ClientSocketHandler.Instance;
            client.onMessageReceived = OnMessageReceived;

            state = new State();
            color = colorIn;
        }

        public void OnMessageReceived(int code, int value)
        {
            if (code == (int) Messages.OnMovePlayed)
            {
                int move = value;
                MoveHelper.ExecuteMove(state, move);

                mainView.SetState(new Neutral(mainView, this, new Move(move)));
            }
        }

        public void Play(Move move)
        {
            client.SendMessage((int) Messages.PlayMove, move.GetMove());
        }
    }
}
