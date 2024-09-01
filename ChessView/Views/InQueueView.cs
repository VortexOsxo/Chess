using ChessView.Widgets;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ChessView.Configs;

namespace ChessView.Views
{
    internal class InQueueView : View
    {
        Text queueText;
        Button leaveQueueButton;

        public InQueueView()
        {
            queueText = new Text("Currently in queue", Config.Font);
            int left = (int)((Config.WindowWidth - queueText.GetGlobalBounds().Width) / 2);
            queueText.Position = new Vector2f(left, 50);

            int buttonLeft = (Config.WindowWidth - Config.ButtonWidth) / 2;

            leaveQueueButton = new Button(new Vector2f(buttonLeft, 300), "Leave Queue");
        }


        public void Draw(RenderWindow window)
        {
            window.Draw(queueText);
            window.Draw(leaveQueueButton);
        }

        public View? OnMousePressed(MouseButtonEventArgs e)
        {
            if (leaveQueueButton.Collide(e.X, e.Y))
            {
                JoinGameService.Instance.LeaveGameQueue();
                return new HomeView();
            }
            return null;
        }

        public View? Update()
        {
            return JoinGameService.Instance.CanJoinGame();
        }
    }
}
