using ChessCore;
using ChessCore.Moves;

namespace ChessView.Views.GameView.ViewState
{
    internal class Neutral : Base
    {
        public Neutral(Move? lastMove = null)
        {
            highlighted = new bool[64];
            if (lastMove != null)
            {
                highlighted[GetDrawPosition(lastMove.GetStartPosition())] = true;
                highlighted[GetDrawPosition(lastMove.GetEndPosition())] = true;
            }
        }

        override public Base? HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int position = mainView.GetIndexClicked(e);
            if (!IsPositionValid(position)) return null;

            int piece = user.state.board[GetDrawPosition(position)];
            if (piece == 0 || (piece & Piece.ColorFilter) != user.color)
                return null;
            return new Selected(GetDrawPosition(position));
        }
    }
}
