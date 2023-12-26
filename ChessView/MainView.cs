using SFML.Graphics;
using SFML.System;
using SFML.Window;

using ChessCore;
using ChessContext;

namespace ChessView
{
    internal class MainView
    {
        private RenderWindow window;
        private readonly Color backgroundColor = new Color(50, 50, 50, 255);
        private readonly Font font = new Font("assets/arial.ttf");

        private readonly int startX;
        private readonly int startY;


        // Tiles Variables
        private const int tileSize = 68;

        private readonly Color lightTilesColor = new Color(238, 238, 210, 255);
        private readonly Color darkTilesColor = new Color(118, 150, 86, 255);
        private readonly Color lightTilesHighlightedColor = new Color(255, 255, 255, 255);
        private readonly Color darkTilesHighlightedColor = new Color(186, 202, 68, 255);

        private readonly RectangleShape tile = new RectangleShape(new Vector2f(tileSize, tileSize));

        //Pieces Variables
        private Dictionary<int, Sprite> piecesSprite = new Dictionary<int, Sprite>();

        // Create a Text object
        private Text text;


        // View State Variables
        private Game game;
        private ViewState.Base state;

        public MainView()
        {
            window = new RenderWindow(new VideoMode(1536, 864), "Chess Board", Styles.Default);
            window.SetFramerateLimit(60);

            window.Closed += (sender, e) => window.Close();
            window.MouseButtonPressed += OnMousePressed;

            startX = ((int)window.Size.X - tileSize * 8) / 2;
            startY = ((int)window.Size.Y - tileSize * 8) / 2;

            text = new Text("", font);
            text.CharacterSize = 24;
            text.FillColor = Color.White;

            SetUpPiecesSprite();

            game = new Game();
            state = new ViewState.Neutral();
            ViewState.Base.SetUp(this, game);
        }

        private void SetUpPiecesSprite()
        {
            string[] colorString = { "w", "b" };
            int[] colorValue = { Piece.White, Piece.Black };
            var colors = colorString.Zip(colorValue, (item1, item2) => new { String = item1, Value= item2 });

            string[] piecesString = { "p", "b", "n", "r", "q", "k" };
            int[] piecesValue = { Piece.Pawn, Piece.Bishop, Piece.Knight, Piece.Rook, Piece.Queen, Piece.King};
            var pieces = piecesString.Zip(piecesValue, (item1, item2) => new { String = item1, Value = item2 });

            foreach (var color in colors)
            {
                foreach (var piece in pieces)
                {
                    Texture texture = new Texture($"assets/{color.String}{piece.String}.png");
                    piecesSprite.Add(color.Value | piece.Value, new Sprite(texture));
                }
            }
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(backgroundColor);

                Draw();
                window.Display();
            }
        }

        private void OnMousePressed(object? sender, SFML.Window.MouseButtonEventArgs e)
        {
            state = state.HandleClick(e);
        }

        public int GetIndexClicked(SFML.Window.MouseButtonEventArgs e)
        {
            return (e.X - startX) / tileSize + (e.Y - startY) / tileSize * 8;
        }

        public int GetChosenPiece(SFML.Window.MouseButtonEventArgs e)
        {
            if (startX + -2 * tileSize > e.X || startX + -1 * tileSize < e.X) return 0;
            for (int i = 2; i < 6; ++i)
            {
                if(startY + i * tileSize < e.Y && startY + (i+1) * tileSize > e.Y)
                {
                    return i | game.GetTeamToPlay();
                }
            }
            return 0;
        }

        private void Draw() 
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    DrawBoard(row, col);
                    DrawPieces(row, col);
                }
            }
            state.Draw(window);
            DrawGameResult();
        }

        private void DrawBoard(int col, int row)
        {
            tile.Position = new Vector2f(startX + col * tileSize, startY + row * tileSize);
            tile.FillColor = (row + col) % 2 == 0
                ? ViewState.Base.highlighted[row * 8 + col] ? lightTilesHighlightedColor : lightTilesColor
                : ViewState.Base.highlighted[row * 8 + col] ? darkTilesHighlightedColor : darkTilesColor;

            window.Draw(tile);
        }

        private void DrawPieces(int col, int row)
        {   
            Sprite? sprite = piecesSprite.GetValueOrDefault(game.GetBoard()[row * 8 + col]);
            if (sprite == null) return;

            sprite.Position = new Vector2f(startX + col * tileSize, startY + row * tileSize);
            window.Draw(sprite);
        }

        public void DrawPieceSelection() 
        {
            for(int i = 2; i < 6; ++i)
            {
                tile.Position = new Vector2f(startX + -2 * tileSize, startY + (i) * tileSize);
                tile.FillColor = i % 2 == 0 ? lightTilesColor : darkTilesColor;
                window.Draw(tile);

                Sprite? sprite = piecesSprite.GetValueOrDefault(i | game.GetTeamToPlay());
                if (sprite == null) return;

                sprite.Position = tile.Position;
                window.Draw(sprite);
            }
        }

        private void DrawGameResult()
        {
            Game.Result result = game.GetGameResult();
            switch(result)
            {
                case Game.Result.InProgress: 
                    text = new Text("Game State: In Progress", font);
                    break;
                case Game.Result.BlackWin:
                    text = new Text("Game State: Black Win", font);
                    break;
                case Game.Result.WhiteWin:
                    text = new Text("Game State: White Win", font);
                    break;
                case Game.Result.Draw:
                    text = new Text("Game State: Draw", font);
                    break;
            }
            window.Draw(text);

        }
    }
}
