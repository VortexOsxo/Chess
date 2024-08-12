using ChessCore.Moves;
using SFML.Graphics;

namespace ChessView.Views.GameView.ViewState
{
    internal class Promotion : Base
    {
        private Move promotionMove;
        public Promotion(Move move)
        {
            promotionMove = move;
            highlighted = new bool[64];
        }

        override public Base? HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int piece = mainView.GetChosenPiece(e);
            if (piece == 0) return new Neutral();

            promotionMove.ChangePromotionType(piece);
            user.Play(promotionMove);
            return new ComputerTurn(promotionMove);
        }

        override public void Draw(RenderWindow window)
        {
            mainView.DrawPieceSelection(window);
        }
    }
}
