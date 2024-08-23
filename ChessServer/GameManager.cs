using ChessCore.AI;

namespace ChessCore.GameContext
{
    internal class GameManager
    {
        static private readonly GameManager instance = new GameManager();
        static public GameManager Instance { get { return instance; } }

        private UserPlayer? inQueue;

        private GameManager() {}

        public void JoinSinglePlayerGame(UserPlayer player)
        {
            new Game(player, new AIPlayer()).Start();
        }

        public void JoinGameQueue(UserPlayer player)
        {
            if (inQueue == null)
            {
                inQueue = player;
                return;
            }

            new Game(player, inQueue).Start();
            inQueue = null;
        }

        public void LeaveGameQueue(UserPlayer player)
        {
            if (inQueue == player)
            {
                inQueue = null;
            }
        }
    }
}
