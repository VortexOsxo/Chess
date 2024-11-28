using ChessCommunication;
using System.Net.Sockets;

namespace ChessView
{
    internal class ClientSocketHandler : SocketHandler
    {
        public static ClientSocketHandler Instance { get; } = new ClientSocketHandler();

        private ClientSocketHandler() : base(new TcpClient("127.0.0.1", 13000)) {}

    }
}
