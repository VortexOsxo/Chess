using System.Net.Sockets;

namespace ChessCommunication
{
    public class SocketHandler
    {
        public Action<byte[]>? onMessageReceived;

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
            byte[] buffer = new byte[256];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    onMessageReceived?.Invoke(buffer);
                }
            }
            finally
            {
                Close();
            }
        }

        public void SendMessage(byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public void Close()
        {
            client.Close();
        }
    }
}
