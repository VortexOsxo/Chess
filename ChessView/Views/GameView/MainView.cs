using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ChessCore;
using ChessView.Views.GameView.ViewState;

namespace ChessView.Views.GameView
{
    internal class MainView : View
    {
        private readonly int startX;
        private readonly int startY;


        // Tiles Variables
        private const int tileSize = 68;

        private readonly RectangleShape tile = new RectangleShape(new Vector2f(tileSize, tileSize));

        //Pieces Variables
        private Dictionary<int, Sprite> piecesSprite = new Dictionary<int, Sprite>();

        // Create a Text object
        private Text text;


        // View State Variables
        private ClientPlayer player;
        private BaseViewState state;

        public MainView()
        {
            startX = (Config.WindowWidth - tileSize * 8) / 2;
            startY = (Config.WindowHeight - tileSize * 8) / 2;

            text = new Text("", Config.Font);
            text.CharacterSize = 24;
            text.FillColor = Color.White;

            SetUpPiecesSprite();

            player  = new ClientPlayer(this);

            SetState(new ComputerTurn(this, player));
        }

        public void SetState(BaseViewState newState)
        {
            state = newState;
        }

        public void Draw(RenderWindow window)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    DrawBoard(window, row, col);
                    if (player.color == Piece.White)
                    {
                        DrawPieces(window, row, col);
                    }
                    else
                    {
                        DrawPiecesReversed(window, row, col);
                    }
                }
            }
            state.Draw(window);
        }

        public View? Update()
        {
            var newState = state.Update();
            if (newState != null) SetState(newState);

            return null;
        }


        public View? OnMousePressed(MouseButtonEventArgs e)
        {
            var newState = state.HandleClick(e);
            if(newState != null) SetState(newState);

            return null;
        }

        private void SetUpPiecesSprite()
        {
            string[] colorString = { "w", "b" };
            int[] colorValue = { Piece.White, Piece.Black };
            var colors = colorString.Zip(colorValue, (item1, item2) => new { String = item1, Value = item2 });

            string[] piecesString = { "p", "b", "n", "r", "q", "k" };
            int[] piecesValue = { Piece.Pawn, Piece.Bishop, Piece.Knight, Piece.Rook, Piece.Queen, Piece.King };
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

        public int GetIndexClicked(MouseButtonEventArgs e)
        {
            return (e.X - startX) / tileSize + (e.Y - startY) / tileSize * 8;
        }

        public int GetChosenPiece(MouseButtonEventArgs e)
        {
            if (startX + -2 * tileSize > e.X || startX + -1 * tileSize < e.X) return 0;
            for (int i = 2; i < 6; ++i)
            {
                if (startY + i * tileSize < e.Y && startY + (i + 1) * tileSize > e.Y)
                {
                    return i | player.color;
                }
            }
            return 0;
        }

        private void DrawBoard(RenderWindow window, int col, int row)
        {
            tile.Position = new Vector2f(startX + col * tileSize, startY + row * tileSize);
            tile.FillColor = (row + col) % 2 == 0
                ? BaseViewState.highlighted[row * 8 + col] ? Config.LightTilesHighlightedColor : Config.LightTilesColor
                : BaseViewState.highlighted[row * 8 + col] ? Config.DarkTilesHighlightedColor : Config.DarkTilesColor;

            window.Draw(tile);
        }

        private void DrawPieces(RenderWindow window, int col, int row)
        {
            Sprite? sprite = piecesSprite.GetValueOrDefault(player.state.board[row* 8 + col]);
            if (sprite == null) return;

            sprite.Position = new Vector2f(startX + col * tileSize, startY + row * tileSize);
            window.Draw(sprite);
        }

        private void DrawPiecesReversed(RenderWindow window, int col, int row)
        {
            Sprite? sprite = piecesSprite.GetValueOrDefault(player.state.board[(7 - row) * 8 + (7 - col)]);
            if (sprite == null) return;

            sprite.Position = new Vector2f(startX + (col) * tileSize, startY + (row) * tileSize);
            window.Draw(sprite);
        }

        public void DrawPieceSelection(RenderWindow window)
        {
            for (int i = 2; i < 6; ++i)
            {
                tile.Position = new Vector2f(startX + -2 * tileSize, startY + i * tileSize);
                tile.FillColor = i % 2 == 0 ? Config.LightTilesColor : Config.DarkTilesColor;
                window.Draw(tile);

                Sprite? sprite = piecesSprite.GetValueOrDefault(i | (player.state.whiteToPlay ? Piece.White : Piece.Black));
                if (sprite == null) return;

                sprite.Position = tile.Position;
                window.Draw(sprite);
            }
        }
    }
}
