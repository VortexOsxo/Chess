using ChessCore.AI;

namespace ChessCore.GameContext
{
    public class GameManager
    {
        private static readonly GameManager instance = new GameManager();

        static public GameManager Instance { get { return instance; } }

        private List<Game> games = new List<Game>();

        private GameManager() { }

        public UserPlayer CreateSoloGame()
        {
            var userPlayer = new UserPlayer();
            var aiPlayer = new AIPlayer();

            var game = new Game(userPlayer, aiPlayer);
            games.Add(game);

            return userPlayer;
        }

    }
}
