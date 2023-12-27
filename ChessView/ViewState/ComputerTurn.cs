
using ChessCore.Moves;

namespace ChessView.ViewState
{
    internal class ComputerTurn : Base
    {
        public ComputerTurn(Move? lastMove)
        {
            highlighted = new bool[64];
            if (lastMove != null)
            {
                highlighted[lastMove.GetStartPosition()] = true;
                highlighted[lastMove.GetEndPosition()] = true;
            }
        }

        override public Base HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            return this;
        }

        public override Base Update()
        {
            Move playedMove = game.PlayComputerMove();
            return new Neutral(playedMove);
        }
    }
}
