﻿using ChessCore;
using ChessCore.Moves;

namespace ChessView.Views.GameView.ViewState
{
    internal class Neutral : Base
    {
        public Neutral(Move? lastMove = null)
        {
            highlighted = new bool[64];
            if (lastMove != null)
            {
                highlighted[lastMove.GetStartPosition()] = true;
                highlighted[lastMove.GetEndPosition()] = true;
            }
        }

        override public Base HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int position = mainView.GetIndexClicked(e);
            if (position < 0 || position > 63) return this;

            int piece = game.GetPiece(position);
            if (piece == 0 || (piece & Piece.ColorFilter) != game.GetTeamToPlay())
                return this;
            return new Selected(position);
        }
    }
}
