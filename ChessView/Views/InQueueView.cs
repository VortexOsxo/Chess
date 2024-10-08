﻿using ChessView.Widgets;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ChessView.Configs;

namespace ChessView.Views
{
    internal class InQueueView : View
    {
        readonly Text queueText;

        readonly ButtonContainer buttons;

        public InQueueView()
        {
            queueText = new Text("Currently in queue", Config.Font);
            var left = (int)((Config.WindowWidth - queueText.GetGlobalBounds().Width) / 2);
            queueText.Position = new Vector2f(left, 50);
            
            buttons = new ButtonContainer();

            var buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

            buttons.Add(new Button(new Vector2f(buttonLeft, 300), "Leave Queue", () => new HomeView()));
        }


        public void Draw(RenderWindow window)
        {
            window.Draw(queueText);
            buttons.Draw(window);
        }

        public View? OnMousePressed(MouseButtonEventArgs e)
        {
            return buttons.HandleClick(e);
        }

        public View? Update()
        {
            return JoinGameService.Instance.CanJoinGame();
        }
    }
}
