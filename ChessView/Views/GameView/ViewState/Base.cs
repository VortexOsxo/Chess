﻿using ChessCore;
using ChessCore.GameContext;
using SFML.Graphics;

namespace ChessView.Views.GameView.ViewState
{
    abstract internal class Base
    {
        static protected MainView mainView;
        static protected ClientPlayer user;

        public static bool[] highlighted;

        static public void SetUp(MainView mainViewIn, ClientPlayer userIn)
        {
            mainView = mainViewIn;
            user = userIn;
        }

        abstract public Base? HandleClick(SFML.Window.MouseButtonEventArgs e);

        virtual public void Draw(RenderWindow window) { }

        virtual public Base? Update() { return null; }

        protected bool IsPositionValid(int position)
        {
            return position > 0 && position < 64;
        }

        protected int GetDrawPosition(int position)
        {
            return user.color == Piece.White ? position : 63 - position;
        }
    }
}
