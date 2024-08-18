
using ChessCore.Moves;

namespace ChessView.Views.GameView.ViewState
{
    internal class ComputerTurn : Base
    {
        public ComputerTurn(Move? lastMove = null)
        {
            highlighted = new bool[64];
            if (lastMove != null)
            {
                highlighted[lastMove.GetStartPosition()] = true;
                highlighted[lastMove.GetEndPosition()] = true;
            }
        }

        override public Base? HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            return null;
        }
    }
}
