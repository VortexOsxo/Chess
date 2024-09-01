using SFML.Graphics;

namespace ChessView
{
    internal static class Config
    {
        public static readonly Font Font = new Font("assets/arial.ttf");

        public static int WindowWidth => BigWindow.WindowWidth;
        public static int WindowHeight => BigWindow.WindowHeight;
        public static int ButtonWidth => BigWindow.ButtonWidth;
        public static int ButtonHeight => BigWindow.ButtonHeight;
        public static uint FontSize => BigWindow.FontSize;
        public static int TileSize => BigWindow.TileSize;


        public static readonly Color BackgroundColor = new Color(50, 50, 50, 255);

        public static readonly Color LightTilesColor = new Color(238, 238, 210, 255);
        public static readonly Color DarkTilesColor = new Color(118, 150, 86, 255);
        public static readonly Color LightTilesHighlightedColor = new Color(255, 255, 255, 255);
        public static readonly Color DarkTilesHighlightedColor = new Color(186, 202, 68, 255);
        
        private static class BigWindow
        {
            public static readonly int WindowHeight = 1350;
            public static readonly int WindowWidth = 2400;

            public static readonly int ButtonWidth = 500;
            public static readonly int ButtonHeight = 75;
            
            public static readonly uint FontSize = 50;
            public static readonly int TileSize = 120;
        }
    }
}
