using ChessCore.Moves;
using SFML.Graphics;

namespace ChessView.Views.GameView.ViewState
{
    internal class Promotion : BaseViewState
    {
        private Move promotionMove;
        public Promotion(MainView mainViewIn, ClientPlayer userIn, Move move) : base(mainViewIn, userIn)
        {
            promotionMove = move;
            highlighted = new bool[64];
        }

        override public BaseViewState? HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int piece = MainView.GetChosenPiece(e);
            if (piece == 0) return new Neutral(MainView, User);

            promotionMove.ChangePromotionType(piece);
            User.Play(promotionMove);
            return new ComputerTurn(MainView, User, promotionMove);
        }

        override public void Draw(RenderWindow window)
        {
            MainView.DrawPieceSelection(window);
        }
    }
}
