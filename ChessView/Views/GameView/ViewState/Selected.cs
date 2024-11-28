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

            possibleMoves = ValidMovesFinder.GetValidMoveFrom(User.State, selected);

            foreach (Move move in possibleMoves)
            {
                highlighted[GetDrawPosition(move.GetEndPosition())] = true;
            }
        }

        override public BaseViewState? HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int position = GetDrawPosition(MainView.GetIndexClicked(e));
            if (!IsPositionValid(position)) return this;

            foreach (Move move in possibleMoves)
            {
                if (position == move.GetEndPosition())
                {
                    if (move.IsPromotion())
                    {
                        return new Promotion(MainView, User, move);
                    }
                    User.Play(move);
                    return new ComputerTurn(MainView, User);
                }
            }

            int piece = User.State.board[position];
            if (piece == 0)
                return new Neutral(MainView, User);
            else if ((piece & Piece.ColorFilter) == (User.State.board[selected] & Piece.ColorFilter))
                return new Selected(MainView, User,position);
            return null;
        }
    }
}
