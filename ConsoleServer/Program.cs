using System;
using ConsoleCommon;
using Deterministic.Network;

namespace ConsoleServer
{
    class Program
    {
        private static TcpPeer _peer;
        static void Main()
        {
            Console.WriteLine("Starting TCP Server 0.0.0.0:50050");
            TcpServer server = new TcpServer(50050);
            server.OnPeerConected += OnConnected;

            Console.ReadLine();
        }

        private static void OnConnected(TcpPeer peer)
        {
            Console.WriteLine("Peer Connected");
            _peer = peer;
            _peer.OnPacketReceived += OnReceived;
        }

        private static void OnReceived(Packet packet)
        {
            Console.WriteLine("Message Received");

            if (packet is ConsoleMessage message)
            {
                Console.WriteLine($"Client: {message.Text}");
                _peer.Send(new ConsoleMessage {Text = message.Text});
            }
        }
    }
}
