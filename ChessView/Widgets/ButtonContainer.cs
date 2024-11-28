using SFML.Graphics;
using SFML.Window;
using View = ChessView.Views.View;

namespace ChessView.Widgets;

public class ButtonContainer
{
    private readonly List<Button> buttons = new();

    public void Add(Button button)
    {
        buttons.Add(button);
    }

    public void Draw(RenderWindow window)
    {
        buttons.ForEach(window.Draw);
    }

    public View? HandleClick(MouseButtonEventArgs e)
    {
        foreach (var button in buttons.Where(button => button.Collide(e.X, e.Y)))
        {
            return button.Click();
        }

        return null;
    }
}