using ChessCommunication;
using ChessView.Views;
using ChessView.Views.GameView;

namespace ChessView
{
    internal class JoinGameService
    {
        bool bJoinedGame = false;
        int color = 0;

        static public JoinGameService Instance {  get { return instance; } }
        static private JoinGameService instance = new JoinGameService();

        public void AttemptToJoinGame(Messages joinGameMessage)
        {
            var instance = ClientSocketHandler.Instance;
            instance.SendMessage((int)joinGameMessage);
            instance.onMessageReceived += (int code, int value) =>
            {
                if (code == (int)Messages.OnGameJoined)
                {
                    bJoinedGame = true;
                    color = value;
                }
            };
        }

        public View? CanJoinGame()
        {
            if (bJoinedGame)
            {
                return new MainView(color);
            }
            return null;
        }

    }
}
