using ChessContext;
using SFML.Graphics;

namespace ChessView.ViewState
{
    abstract internal class Base
    {
        static protected MainView mainView;
        static protected Game game;

        public static bool[] highlighted;

        public Base() 
        {
        }

        static public void SetUp(MainView mainViewIn, Game gameIn)
        {
            mainView = mainViewIn;
            game = gameIn;
        }

        abstract public Base HandleClick(SFML.Window.MouseButtonEventArgs e);

        virtual public void Draw(RenderWindow window) { }
    }
}
