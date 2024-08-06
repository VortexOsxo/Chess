using SFML.Window;
using SFML.Graphics;
using SFML.System;
using ChessView.Views.GameView;

namespace ChessView.Views
{
    internal class HomeView : View
    {
        Text homeText;

        HomeButton singleplayerButton;
        HomeButton multiplayerButton;
        HomeButton passPlayButton;


        public HomeView()
        {
            homeText = new Text("Welcome to Chess", Config.Font);
            int left = (int)((Config.WindowWidth - homeText.GetGlobalBounds().Width) / 2);
            homeText.Position = new Vector2f(left, 50);

            int buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

            singleplayerButton = new HomeButton(new Vector2f(buttonLeft, 300), new Text("SinglePlayer", Config.Font));
            multiplayerButton = new HomeButton(new Vector2f(buttonLeft, 400), new Text("MultiPlayer", Config.Font));
            passPlayButton = new HomeButton(new Vector2f(buttonLeft, 500), new Text("Pass and Play", Config.Font));
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
            }
            return null;
        }

        public View? Update()
        {
            return null;
        }



        private class HomeButton : Drawable
        {
            public Text text;
            public RectangleShape shape;

            public HomeButton(Vector2f position, Text textInput)
            {
                shape = new RectangleShape(new Vector2f(200f, 50f));
                shape.FillColor = Config.DarkTilesColor;
                shape.Position = position;

                text = textInput;
                text.Position = position;
            }

            public void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(shape, states);
                target.Draw(text, states);
            }

            public bool Collide(int x, int y)
            {
                return shape.GetGlobalBounds().Contains(x, y);
            }
        }
    }
}
