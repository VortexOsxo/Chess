using SFML.Window;
using SFML.Graphics;
using SFML.System;
using ChessView.Widgets;
using ChessCommunication;

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
            homeText = Utils.CreateText("Welcome to Chess", new Vector2f(0, 50));
            int left = (int)((Config.WindowWidth - homeText.GetGlobalBounds().Width) / 2);
            homeText.Position = new Vector2f(left, 50);


            int buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

            singleplayerButton = new Button(new Vector2f(buttonLeft, 300), "SinglePlayer");
            multiplayerButton = new Button(new Vector2f(buttonLeft, 400), "MultiPlayer");
            passPlayButton = new Button(new Vector2f(buttonLeft, 500), "Pass and Play");
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
                JoinGameService.Instance.AttemptToJoinGame(Messages.JoinSinglePlayerGame);
            } 
            
            else if (multiplayerButton.Collide(e.X, e.Y))
            {
                JoinGameService.Instance.AttemptToJoinGame(Messages.JoinMultiPlayerGame);
                return new InQueueView();
            }
            return null;
        }

        public View? Update()
        {
            return JoinGameService.Instance.CanJoinGame();
        }
    }
}
