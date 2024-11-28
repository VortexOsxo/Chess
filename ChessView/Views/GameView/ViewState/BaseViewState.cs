using ChessCore;
using SFML.Graphics;

namespace ChessView.Views.GameView.ViewState
{
    internal abstract class BaseViewState
    {
        protected readonly MainView MainView;
        protected readonly ClientPlayer User;

        public static bool[] highlighted = [];

        protected BaseViewState(MainView mainViewIn, ClientPlayer userIn)
        {
            MainView = mainViewIn;
            User = userIn;
        }

        public abstract BaseViewState? HandleClick(SFML.Window.MouseButtonEventArgs e);

        public virtual void Draw(RenderWindow window) { }

        protected bool IsPositionValid(int position)
        {
            return position is > 0 and < 64;
        }

        protected int GetDrawPosition(int position)
        {
            return User.Color == Piece.White ? position : 63 - position;
        }
    }
}
