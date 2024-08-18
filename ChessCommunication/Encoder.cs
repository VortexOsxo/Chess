namespace ChessCommunication
{
    public class Encoder
    {
        static public readonly byte[] JoinGame = [1];

        static public byte[] EncodeMove(int move)
        {
            return [2, 1, (byte)((move >> 24) & 0xff), (byte)((move >> 16) & 0xff), (byte)((move >> 8) & 0xff), (byte)(move & 0xff) ];
        }

        static public int DecodeMove(byte[] move)
        {
            return (move[2] << 24) | (move[3] << 16) | (move[4] << 8) | move[5];
        }
    }
}
