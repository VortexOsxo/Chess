using SFML.Graphics;
using SFML.System;

namespace ChessView.Widgets
{
    public class Button : Drawable
    {
        public Text text;
        public RectangleShape shape;

        public Button(Vector2f position, Text textInput)
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
