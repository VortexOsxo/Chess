namespace ChessView.Views;

using Configs.WindowSizes;
using Widgets;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Configs;

public class SettingView : View
{
    readonly Text welcomeText;

    readonly ButtonContainer buttons;
    
    public SettingView()
    {
        welcomeText = Utils.CreateText("Change window size", new Vector2f(0, 50));
        var left = (int)((Config.WindowWidth - welcomeText.GetGlobalBounds().Width) / 2);
        welcomeText.Position = new Vector2f(left, 50);
        
        buttons = new ButtonContainer();
        
        var buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

        buttons.Add(new Button(new Vector2f(buttonLeft, 300), "Small Window", () =>
        {
            Config.SetWindowSize(new SmallWindowSize());
            return new HomeView();
        }));
        buttons.Add( new Button(new Vector2f(buttonLeft, 400), "Medium Window", () => new HomeView()));
        buttons.Add(new Button(new Vector2f(buttonLeft, 500), "Big Window", () =>
        {
            Config.SetWindowSize(new BigWindowSize());
            return new HomeView();
        }));
    }
    public void Draw(RenderWindow window)
    {
        buttons.Draw(window);
        window.Draw(welcomeText);
    }

    public View? Update()
    {
        return null;
    }

    public View? OnMousePressed(MouseButtonEventArgs e)
    {
        return buttons.HandleClick(e);
    }
}