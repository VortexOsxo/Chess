using ChessCore;
using SFML.Graphics;

namespace ChessView.Views.GameView.ViewState
{
    abstract internal class BaseViewState
    {
        protected MainView mainView;
        protected ClientPlayer user;

        public static bool[] highlighted;

        public BaseViewState(MainView mainViewIn, ClientPlayer userIn)
        {
            mainView = mainViewIn;
            user = userIn;
        }

        abstract public BaseViewState? HandleClick(SFML.Window.MouseButtonEventArgs e);

        virtual public void Draw(RenderWindow window) { }

        virtual public BaseViewState? Update() { return null; }

        protected bool IsPositionValid(int position)
        {
            return position > 0 && position < 64;
        }

        protected int GetDrawPosition(int position)
        {
            return user.color == Piece.White ? position : 63 - position;
        }
    }
}
