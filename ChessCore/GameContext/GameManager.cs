using ChessCore.AI;

namespace ChessCore.GameContext
{
    public class GameManager
    {
        private static readonly GameManager instance = new GameManager();
        static public GameManager Instance { get { return instance; } }


        private Dictionary<int, Game> games = new ();

        private GameManager() { }

        public UserPlayer CreateSoloGame()
        {
            var userPlayer = new UserPlayer();
            var aiPlayer = new AIPlayer();

            var game = new Game(userPlayer, aiPlayer);
            games.Add(0, game);

            return userPlayer;
        }

        public Game? GetGame(int gameNumber)
        {
            return games[gameNumber];
        }

    }
}
