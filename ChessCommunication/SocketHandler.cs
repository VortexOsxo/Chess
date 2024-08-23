using System.Net.Sockets;

namespace ChessCommunication
{
    public class SocketHandler
    {
        public Action<int, int>? onMessageReceived;

        private readonly TcpClient client;
        private readonly NetworkStream stream;
        private readonly Thread receiveThread;

        public SocketHandler(TcpClient clientIn)
        {
            client = clientIn;
            stream = client.GetStream();

            receiveThread = new Thread(() => ReceiveMessages(stream));
            receiveThread.Start();
        }

        private void ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[8];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    Tuple<int, int> message = Encoder.DecodeMessage(buffer);
                    onMessageReceived?.Invoke(message.Item1, message.Item2);
                }
            }
            finally
            {
                Close();
            }
        }

        public void SendMessage(int code, int value = 0)
        {
            byte[] data = Encoder.EncodeMessage(code, value);
            stream.Write(data, 0, data.Length);
        }

        public void Close()
        {
            client.Close();
        }
    }
}
