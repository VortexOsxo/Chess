using ChessCommunication;
using ChessCore.GameContext;
using ChessServer;
using System.Net;
using System.Net.Sockets;

class Server
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 13000);
        server.Start();

        Console.WriteLine("Server started, waiting for connections...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();

            var player = new UserPlayer(new SocketHandler(client));
        }
    }
}