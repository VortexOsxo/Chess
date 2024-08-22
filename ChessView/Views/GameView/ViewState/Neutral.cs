using ChessCore;
using ChessCore.Moves;

namespace ChessView.Views.GameView.ViewState
{
    internal class Neutral : BaseViewState
    {
        public Neutral(MainView mainViewIn, ClientPlayer userIn, Move? lastMove = null) : base(mainViewIn, userIn)
        {
            highlighted = new bool[64];
            if (lastMove != null)
            {
                highlighted[GetDrawPosition(lastMove.GetStartPosition())] = true;
                highlighted[GetDrawPosition(lastMove.GetEndPosition())] = true;
            }
        }

        override public BaseViewState? HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int position = GetDrawPosition(mainView.GetIndexClicked(e));
            if (!IsPositionValid(position)) return null;

            int piece = user.state.board[position];
            if (piece == 0 || (piece & Piece.ColorFilter) != user.color)
                return null;
            return new Selected(mainView, user, position);
        }
    }
}
