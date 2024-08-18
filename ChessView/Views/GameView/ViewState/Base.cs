using ChessCore.GameContext;
using SFML.Graphics;

namespace ChessView.Views.GameView.ViewState
{
    abstract internal class Base
    {
        static protected MainView mainView;
        static protected ClientPlayer user;

        public static bool[] highlighted;

        static public void SetUp(MainView mainViewIn, ClientPlayer userIn)
        {
            mainView = mainViewIn;
            user = userIn;
        }

        abstract public Base? HandleClick(SFML.Window.MouseButtonEventArgs e);

        virtual public void Draw(RenderWindow window) { }

        virtual public Base? Update() { return null; }
    }
}
