using ChessCommunication;
using System.Net.Sockets;

namespace ChessView
{
    internal class ClientSocketHandler : SocketHandler
    {
        static public ClientSocketHandler Instance { get { return instance; } }
        static private ClientSocketHandler instance = new ClientSocketHandler();

        private ClientSocketHandler() : base(new TcpClient("127.0.0.1", 13000)) {}

    }
}
