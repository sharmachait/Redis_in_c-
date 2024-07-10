using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace codecrafters_redis
{
    class TcpServer
    {
        private readonly TcpListener _server;

        public TcpServer(IPAddress ipAddress, int port)
        {
            _server = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            _server.Start();
            Socket clientSocket = _server.AcceptSocket();
            clientSocket.SendAsync(Encoding.UTF8.GetBytes("+PONG\r\n"));
        }

        public void Stop()
        {
            _server.Stop();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TcpServer server = new TcpServer(IPAddress.Any, 6379);
            server.Start();
        }
    }
}