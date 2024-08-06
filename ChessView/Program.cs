using ChessView;
using SFML.Graphics;
using SFML.Window;

class Program
{
    static private readonly Color backgroundColor = new Color(50, 50, 50, 255);
    static private ChessView.Views.View? view;

    static void Main()
    {
        RenderWindow window = new(new VideoMode((uint)Config.WindowWidth, (uint)Config.WindowHeight), "Chess", Styles.Default);
        window.SetFramerateLimit(60);

        window.Closed += (sender, e) => window.Close();
        window.MouseButtonPressed += OnMousePressed;

         view = new ChessView.Views.HomeView();

        while (window.IsOpen)
        {
            window.DispatchEvents();

            window.Clear(backgroundColor);
            view.Draw(window);
            window.Display();

            view = view.Update() ?? view;
        }
    }

    static void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        view = view?.OnMousePressed(e) ?? view;
    }
}
