
using ChessCore.Moves;

namespace ChessView.Views.GameView.ViewState
{
    internal class ComputerTurn : BaseViewState
    {
        public ComputerTurn(MainView mainViewIn, ClientPlayer userIn, Move? lastMove = null) : base(mainViewIn, userIn)
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
            return null;
        }
    }
}
