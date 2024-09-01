using ChessView.Widgets;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ChessView.Views;

public class SettingView : View
{
    Text welcomeText;

    Button smallButton;
    Button mediumButton;
    Button bigButton;
    
    public SettingView()
    {
        welcomeText = Utils.CreateText("Change window size", new Vector2f(0, 50));
        int left = (int)((Config.WindowWidth - welcomeText.GetGlobalBounds().Width) / 2);
        welcomeText.Position = new Vector2f(left, 50);
        
        int buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

        smallButton = new Button(new Vector2f(buttonLeft, 300), "Small Window");
        mediumButton = new Button(new Vector2f(buttonLeft, 400), "Medium Window");
        bigButton = new Button(new Vector2f(buttonLeft, 500), "Big Window");
    }
    public void Draw(RenderWindow window)
    {
        window.Draw(smallButton);
        window.Draw(mediumButton);
        window.Draw(bigButton);
    }

    public View? Update()
    {
        return null;
    }

    public View? OnMousePressed(MouseButtonEventArgs e)
    {
        return new HomeView();
    }
}