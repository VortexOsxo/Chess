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
            int piece = mainView.GetChosenPiece(e);
            if (piece == 0) return new Neutral(mainView, user);

            promotionMove.ChangePromotionType(piece);
            user.Play(promotionMove);
            return new ComputerTurn(mainView, user, promotionMove);
        }

        override public void Draw(RenderWindow window)
        {
            mainView.DrawPieceSelection(window);
        }
    }
}
