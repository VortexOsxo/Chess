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
        Button settingButton;

        public HomeView()
        {
            homeText = Utils.CreateText("Welcome to Chess", new Vector2f(0, 50));
            int left = (int)((Config.WindowWidth - homeText.GetGlobalBounds().Width) / 2);
            homeText.Position = new Vector2f(left, 50);


            int buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

            singleplayerButton = new Button(new Vector2f(buttonLeft, 300), "SinglePlayer");
            multiplayerButton = new Button(new Vector2f(buttonLeft, 400), "MultiPlayer");
            passPlayButton = new Button(new Vector2f(buttonLeft, 500), "Pass and Play");
            settingButton = new Button(new Vector2f(buttonLeft, 600), "Settings");
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(homeText);
            window.Draw(singleplayerButton);
            window.Draw(multiplayerButton);
            window.Draw(passPlayButton);
            window.Draw(settingButton);
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
            
            else if (settingButton.Collide(e.X, e.Y))
            {
                return new SettingView();
            }
            return null;
        }

        public View? Update()
        {
            return JoinGameService.Instance.CanJoinGame();
        }
    }
}
