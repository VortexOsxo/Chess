namespace ChessCommunication
{
    public class Encoder
    {
        static public Tuple<int, int> DecodeMessage(byte[] bytes)
        {
            if (bytes.Length != 8)
            {
                throw new ArgumentException("Byte array must have exactly 8 elements.");
            }

            int code = (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
            int value = (bytes[4] << 24) | (bytes[5] << 16) | (bytes[6] << 8) | bytes[7];

            return new Tuple<int, int>(code, value);
        }

        static public byte[] EncodeMessage(int code, int value)
        {
            return [
                (byte)((code >> 24) & 0xff), (byte)((code >> 16) & 0xff), (byte)((code >> 8) & 0xff), (byte)(code & 0xff),
                (byte)((value >> 24) & 0xff), (byte)((value >> 16) & 0xff), (byte)((value >> 8) & 0xff), (byte)(value & 0xff),
           ];
        }
    }
}
