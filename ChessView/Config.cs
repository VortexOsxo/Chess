using SFML.Graphics;

namespace ChessView
{
    internal class Config
    {
        public static Font Font = new Font("assets/arial.ttf");
        
        public static int WindowHeight = 864;
        public static int WindowWidth = 1536;

        public static int ButtonWidth = 200;

        public static readonly Color BackgroundColor = new Color(50, 50, 50, 255);

        public static readonly Color LightTilesColor = new Color(238, 238, 210, 255);
        public static readonly Color DarkTilesColor = new Color(118, 150, 86, 255);
        public static readonly Color LightTilesHighlightedColor = new Color(255, 255, 255, 255);
        public static readonly Color DarkTilesHighlightedColor = new Color(186, 202, 68, 255);
    }
}
