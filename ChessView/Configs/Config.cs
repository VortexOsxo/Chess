using ChessView.Configs.WindowSizes;
using SFML.Graphics;

namespace ChessView.Configs;
    
internal static class Config
{
    public static readonly Font Font = new Font("assets/arial.ttf");
    public static readonly Color BackgroundColor = new Color(50, 50, 50, 255);

    public static readonly Color LightTilesColor = new Color(238, 238, 210, 255);
    public static readonly Color DarkTilesColor = new Color(118, 150, 86, 255);
    public static readonly Color LightTilesHighlightedColor = new Color(255, 255, 255, 255);
    public static readonly Color DarkTilesHighlightedColor = new Color(186, 202, 68, 255);
    
    
    // Window Size Configs / Logic
    public static Action? OnWindowSizeChanged = null; 
    
    public static int WindowWidth => windowSize.WindowWidth;
    public static int WindowHeight => windowSize.WindowHeight;
    public static int ButtonWidth => windowSize.ButtonWidth;
    public static int ButtonHeight => windowSize.ButtonHeight;
    public static uint FontSize => windowSize.FontSize;
    public static int TileSize => windowSize.TileSize;

    public static void SetWindowSize(WindowSize windowSizeIn)
    {
        windowSize = windowSizeIn;
        OnWindowSizeChanged?.Invoke();
    }
    
    private static WindowSize windowSize = new SmallWindowSize();
}
