using ChessCore;
using ChessCore.Moves;
using ChessCore.FindValidMoves;

namespace ChessView.Views.GameView.ViewState
{
    internal class Selected : BaseViewState
    {
        private int selected;
        private List<Move> possibleMoves;

        public Selected(MainView mainViewIn, ClientPlayer userIn, int selected) : base(mainViewIn, userIn) 
        {
            this.selected = selected;

            highlighted = new bool[64];
            highlighted[GetDrawPosition(selected)] = true;

            possibleMoves = ValidMovesFinder.GetValidMoveFrom(user.state, selected);

            foreach (Move move in possibleMoves)
            {
                highlighted[GetDrawPosition(move.GetEndPosition())] = true;
            }
        }

        override public BaseViewState? HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int position = GetDrawPosition(mainView.GetIndexClicked(e));
            if (!IsPositionValid(position)) return this;

            foreach (Move move in possibleMoves)
            {
                if (position == move.GetEndPosition())
                {
                    if (move.IsPromotion())
                    {
                        return new Promotion(mainView, user, move);
                    }
                    user.Play(move);
                    return new ComputerTurn(mainView, user);
                }
            }

            int piece = user.state.board[position];
            if (piece == 0)
                return new Neutral(mainView, user);
            else if ((piece & Piece.ColorFilter) == (user.state.board[selected] & Piece.ColorFilter))
                return new Selected(mainView, user,position);
            return null;
        }
    }
}
