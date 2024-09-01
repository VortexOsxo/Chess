using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ChessCore;
using ChessView.Views.GameView.ViewState;
using ChessView.Configs;

namespace ChessView.Views.GameView
{
    internal class MainView : View
    {
        private readonly int startX;
        private readonly int startY;

        private readonly RectangleShape tile = new RectangleShape(new Vector2f(Config.TileSize, Config.TileSize));

        //Pieces Variables
        private Dictionary<int, Sprite> piecesSprite = new Dictionary<int, Sprite>();
        
        // View State Variables
        private ClientPlayer player;
        private BaseViewState state;

        public MainView(int color)
        {
            startX = (Config.WindowWidth - Config.TileSize * 8) / 2;
            startY = (Config.WindowHeight - Config.TileSize * 8) / 2;

            SetUpPiecesSprite();

            player  = new ClientPlayer(this, color);

            SetState(color == Piece.White ? new Neutral(this, player) : new ComputerTurn(this, player));
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
                    Sprite sprite = new Sprite(texture);
                    sprite.Scale = new Vector2f(Config.TileSize / 68f, Config.TileSize / 68f);
                    piecesSprite.Add(color.Value | piece.Value, sprite);
                }
            }
        }

        public int GetIndexClicked(MouseButtonEventArgs e)
        {
            return (e.X - startX) / Config.TileSize + (e.Y - startY) / Config.TileSize * 8;
        }

        public int GetChosenPiece(MouseButtonEventArgs e)
        {
            if (startX + -2 * Config.TileSize > e.X || startX + -1 * Config.TileSize < e.X) return 0;
            for (int i = 2; i < 6; ++i)
            {
                if (startY + i * Config.TileSize < e.Y && startY + (i + 1) * Config.TileSize > e.Y)
                {
                    return i | player.color;
                }
            }
            return 0;
        }

        private void DrawBoard(RenderWindow window, int col, int row)
        {
            tile.Position = new Vector2f(startX + col * Config.TileSize, startY + row * Config.TileSize);
            tile.FillColor = (row + col) % 2 == 0
                ? BaseViewState.highlighted[row * 8 + col] ? Config.LightTilesHighlightedColor : Config.LightTilesColor
                : BaseViewState.highlighted[row * 8 + col] ? Config.DarkTilesHighlightedColor : Config.DarkTilesColor;

            window.Draw(tile);
        }

        private void DrawPieces(RenderWindow window, int col, int row)
        {
            Sprite? sprite = piecesSprite.GetValueOrDefault(player.state.board[row* 8 + col]);
            if (sprite == null) return;

            sprite.Position = new Vector2f(startX + col * Config.TileSize, startY + row * Config.TileSize);
            window.Draw(sprite);
        }

        private void DrawPiecesReversed(RenderWindow window, int col, int row)
        {
            Sprite? sprite = piecesSprite.GetValueOrDefault(player.state.board[(7 - row) * 8 + (7 - col)]);
            if (sprite == null) return;

            sprite.Position = new Vector2f(startX + (col) * Config.TileSize, startY + (row) * Config.TileSize);
            window.Draw(sprite);
        }

        public void DrawPieceSelection(RenderWindow window)
        {
            for (int i = 2; i < 6; ++i)
            {
                tile.Position = new Vector2f(startX + -2 * Config.TileSize, startY + i * Config.TileSize);
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
