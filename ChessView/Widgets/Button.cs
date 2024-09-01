using SFML.Graphics;
using SFML.System;
using View = ChessView.Views.View;

namespace ChessView.Widgets
{
    public class Button : Drawable
    {
        public Func<View>? OnClick = null;
        
        private Text text;
        private RectangleShape shape;
        

        public Button(Vector2f position, string textInput)
        {
            shape = new RectangleShape(new Vector2f(Config.ButtonWidth, Config.ButtonHeight));
            shape.FillColor = Config.DarkTilesColor;
            shape.Position = position;

            text = Utils.CreateText(textInput, position);
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
