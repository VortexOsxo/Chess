using ChessCommunication;
using ChessView.Views;
using ChessView.Views.GameView;

namespace ChessView
{
    internal class JoinGameService
    {
        bool bJoinedGame = false;
        int color = 0;

        public static JoinGameService Instance { get; } = new JoinGameService();

        public void AttemptToJoinGame(Messages joinGameMessage)
        {
            var clientSocketHandler = ClientSocketHandler.Instance;
            clientSocketHandler.SendMessage((int)joinGameMessage);
            clientSocketHandler.onMessageReceived += MessageCallback;
        }

        public void LeaveGameQueue()
        {
            var clientSocketHandler = ClientSocketHandler.Instance;
            clientSocketHandler.SendMessage((int)Messages.LeaveQueue);
            clientSocketHandler.onMessageReceived -= MessageCallback;
        }

        public View? CanJoinGame()
        {
            return !bJoinedGame ? null : new MainView(color);
        }

        private void MessageCallback(int code, int value)
        {
            if (code != (int)Messages.OnGameJoined) return;
            bJoinedGame = true;
            color = value;
        }
    }
}
