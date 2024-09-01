using SFML.Window;
using SFML.Graphics;
using SFML.System;
using ChessView.Widgets;
using ChessCommunication;
using ChessView.Configs;

namespace ChessView.Views;

internal class HomeView : View
{
    readonly Text homeText;
    readonly ButtonContainer buttons = new();
    
    public HomeView()
    {
        homeText = Utils.CreateText("Welcome to Chess", new Vector2f(0, 50));
        var left = (int)((Config.WindowWidth - homeText.GetGlobalBounds().Width) / 2);
        homeText.Position = new Vector2f(left, 50);


        var buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

        buttons.Add(new Button(new Vector2f(buttonLeft, 300), "SinglePlayer", () => {
            JoinGameService.Instance.AttemptToJoinGame(Messages.JoinSinglePlayerGame);
            return null;
        }));
        buttons.Add(new Button(new Vector2f(buttonLeft, 400), "MultiPlayer", () =>
        {
            JoinGameService.Instance.AttemptToJoinGame(Messages.JoinMultiPlayerGame);
            return new InQueueView();
        }));
        buttons.Add(new Button(new Vector2f(buttonLeft, 500), "Pass and Play", () => null ));
        buttons.Add(new Button(new Vector2f(buttonLeft, 600), "Settings", () => new SettingView()));
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(homeText);
        buttons.Draw(window);
    }

    public View? OnMousePressed(MouseButtonEventArgs e)
    {
        return buttons.HandleClick(e);
    }

    public View? Update()
    {
        return JoinGameService.Instance.CanJoinGame();
    }
}
