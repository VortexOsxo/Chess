using ChessView;
using SFML.Graphics;
using SFML.Window;
using ChessView.Configs;
using SFML.System;

class Program
{
    private static ChessView.Views.View? view;

    static void Main()
    {
        RenderWindow window = CreateWindow();

        Config.OnWindowSizeChanged = () =>
        {
            window.Size = new Vector2u((uint)Config.WindowWidth, (uint)Config.WindowHeight);
        };
        
        view = new ChessView.Views.HomeView();

        while (window.IsOpen)
        {
            window.DispatchEvents();

            window.Clear(Config.BackgroundColor);
            view.Draw(window);
            window.Display();

            view = view.Update() ?? view;
        }

        window.Close();
        window.Dispose();

        ClientSocketHandler.Instance.Close();
    }

    static void OnMousePressed(object? sender, MouseButtonEventArgs e)
    {
        view = view?.OnMousePressed(e) ?? view;
    }

    static RenderWindow CreateWindow()
    {
        RenderWindow window = new(new VideoMode((uint)Config.WindowWidth, (uint)Config.WindowHeight), "Chess", Styles.Close);
        window.SetFramerateLimit(60);

        window.Closed += (sender, e) => window.Close();
        window.MouseButtonPressed += OnMousePressed;
        
        return window;
    }
}
