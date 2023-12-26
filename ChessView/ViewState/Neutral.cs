using ChessCore;

namespace ChessView.ViewState
{
    internal class Neutral : Base
    {
        public Neutral()
        {
            highlighted = new bool[64];
        }

        override public Base HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int position = mainView.GetIndexClicked(e);
            if (position < 0 || position > 63) return this;

            int piece = game.GetPiece(position);
            if (piece == 0 || (piece & Piece.ColorFilter) != game.GetTeamToPlay())
                return this;
            return new Selected(position);
        }
    }
}
