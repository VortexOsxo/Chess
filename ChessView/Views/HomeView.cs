using SFML.Window;
using SFML.Graphics;
using SFML.System;
using ChessView.Views.GameView;
using ChessView.Widgets;

namespace ChessView.Views
{
    internal class HomeView : View
    {
        Text homeText;

        Button singleplayerButton;
        Button multiplayerButton;
        Button passPlayButton;


        public HomeView()
        {
            homeText = new Text("Welcome to Chess", Config.Font);
            int left = (int)((Config.WindowWidth - homeText.GetGlobalBounds().Width) / 2);
            homeText.Position = new Vector2f(left, 50);

            int buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

            singleplayerButton = new Button(new Vector2f(buttonLeft, 300), new Text("SinglePlayer", Config.Font));
            multiplayerButton = new Button(new Vector2f(buttonLeft, 400), new Text("MultiPlayer", Config.Font));
            passPlayButton = new Button(new Vector2f(buttonLeft, 500), new Text("Pass and Play", Config.Font));
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(homeText);
            window.Draw(singleplayerButton);
            window.Draw(multiplayerButton);
            window.Draw(passPlayButton);
        }

        public View? OnMousePressed(MouseButtonEventArgs e)
        {
            if (singleplayerButton.Collide(e.X, e.Y))
            {
                return new MainView();
            } else if (multiplayerButton.Collide(e.X, e.Y))
            {
                return new InQueueView();
            }
            return null;
        }

        public View? Update()
        {
            return null;
        }
    }
}
