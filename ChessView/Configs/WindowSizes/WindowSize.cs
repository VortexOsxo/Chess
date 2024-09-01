namespace ChessView.Configs.WindowSizes;

public interface WindowSize
{
    public int WindowWidth { get; }
    public int WindowHeight { get; }
    
    public int ButtonWidth { get; }
    public int ButtonHeight { get; }
    
    public uint FontSize { get; }
    public int TileSize { get; }
}

public class BigWindowSize : WindowSize
{
    public int WindowHeight => 1350;
    public int WindowWidth => 2400;

    public int ButtonWidth => 500;
    public int ButtonHeight => 75;
        
    public uint FontSize => 50;
    public int TileSize => 120;
}

public class SmallWindowSize : WindowSize
{
    public int WindowHeight => 864;
    public int WindowWidth => 1536;

    public int ButtonWidth => 200;
    public int ButtonHeight => 50;
        
    public uint FontSize => 25;
    public int TileSize => 68; 
}