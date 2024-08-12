﻿using ChessCore;
using ChessCore.Moves;

namespace ChessView.Views.GameView.ViewState
{
    internal class Selected : Base
    {
        private int selected;
        private List<Move> possibleMoves;
        public Selected(int selected)
        {
            this.selected = selected;

            highlighted = new bool[64];
            highlighted[selected] = true;

            possibleMoves = user.game.GetValidMoveFrom(selected);

            foreach (Move move in possibleMoves)
            {
                highlighted[move.GetEndPosition()] = true;
            }

        }

        override public Base? HandleClick(SFML.Window.MouseButtonEventArgs e)
        {
            int position = mainView.GetIndexClicked(e);
            if (position < 0 || position > 63) return this;

            foreach (Move move in possibleMoves)
            {
                if (position == move.GetEndPosition())
                {
                    if (move.IsPromotion())
                    {
                        return new Promotion(move);
                    }
                    user.Play(move);
                    return null; // Was Computer move
                }
            }

            int piece = user.game.GetPiece(position);
            if (piece == 0)
                return new Neutral();
            else if ((piece & Piece.ColorFilter) == (user.game.GetPiece(selected) & Piece.ColorFilter))
                return new Selected(position);
            return null;
        }
    }
}
