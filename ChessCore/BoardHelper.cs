
namespace ChessCore
{
    internal class BoardHelper
    {
        static public bool IsNotGettingOutOfTheBoard(int position, int movement)
        {
            int new_position = position + movement;
            if (new_position < 0 || new_position > 63) { return false; }

            switch (movement)
            {
                // Diagonal Moves
                case 7:
                case 9:
                case -7:
                case -9:
                    return Math.Abs(new_position / 8 - position / 8) == 1 && Math.Abs(new_position % 8 - position % 8) == 1;

                // Line Moves
                case 8:
                case -8:
                    // May be useless
                    return Math.Abs(new_position / 8 - position / 8) == 1 && Math.Abs(new_position % 8 - position % 8) == 0;

                case 1:
                case -1:
                    return Math.Abs(new_position / 8 - position / 8) == 0 && Math.Abs(new_position % 8 - position % 8) == 1;

                // Knight Moves
                case -17:
                case -15:
                case 15:
                case 17:
                    return Math.Abs(new_position / 8 - position / 8) == 2 && Math.Abs(new_position % 8 - position % 8) == 1;

                case 6:
                case -6:
                case 10:
                case -10:
                    return Math.Abs(new_position / 8 - position / 8) == 1 && Math.Abs(new_position % 8 - position % 8) == 2;

                case 16:
                case -16:
                    // May be uselesss
                    return Math.Abs(new_position / 8 - position / 8) == 1 && Math.Abs(new_position % 8 - position % 8) == 0;

                default: return false;
            }
        }
    }
}
