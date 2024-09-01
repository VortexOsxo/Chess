using SFML.Graphics;
using SFML.System;

namespace ChessView.Widgets;

public static class Utils
{
    public static Text CreateText(string text, Vector2f position)
    {
        var textObj = new Text(text, Config.Font);
        textObj.CharacterSize = Config.FontSize;
        textObj.Position = position;
        return textObj;
    }
}