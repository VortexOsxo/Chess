using SFML.Graphics;
using SFML.System;
using View = ChessView.Views.View;
using ChessView.Configs;

namespace ChessView.Widgets;

public class Button : Drawable
{
    private readonly Func<View?> onClick;
    
    private Text text;
    private RectangleShape shape;
    

    public Button(Vector2f position, string textInput, Func<View> onClickIn)
    {
        shape = new RectangleShape(new Vector2f(Config.ButtonWidth, Config.ButtonHeight));
        shape.FillColor = Config.DarkTilesColor;
        shape.Position = position;

        text = Utils.CreateText(textInput, position);
        
        onClick = onClickIn;
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

    public View? Click()
    {
        return onClick.Invoke();
    }
}
