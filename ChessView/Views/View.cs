using SFML.Graphics;
using SFML.Window;

namespace ChessView.Views
{
    public interface View
    {
        void Draw(RenderWindow window);

        View? Update();

        View? OnMousePressed(MouseButtonEventArgs e);
    }
}
