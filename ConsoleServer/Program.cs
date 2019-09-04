using System;
using ConsoleCommon;
using Deterministic.Network;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
        
            TcpServer server = new TcpServer(50050);
            server.OnPeerConected += OnConected;

            Console.ReadLine();
        }

        private static void OnConected(TcpPeer peer)
        {
            Console.WriteLine("Peer Conected");
            peer.OnPacketReceived += OnReceived;
        }

        private static void OnReceived(Packet packet)
        {
            Console.WriteLine("Message Received");
            Console.WriteLine(packet);

            if (packet is ConsoleMessage message)
            {
                Console.WriteLine($"Client: {message.Text}");
            }
        }
    }
}
