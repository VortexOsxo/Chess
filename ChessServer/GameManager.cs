using ChessCore.AI;

namespace ChessCore.GameContext
{
    internal class GameManager
    {
        private static readonly GameManager instance = new GameManager();
        static public GameManager Instance { get { return instance; } }

        private GameManager() { }
    }
}
