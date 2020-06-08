using System;

using System.Net;
using ConsoleCommon;
using Deterministic.Network;

namespace ConsoleClient
{
    class Program
    {
        static void Main()
        {
            TcpPeer peer = new TcpPeer();
            peer.Connect(IPAddress.Loopback, 50050);
            peer.OnConnected += () =>
            {
                Console.WriteLine("Connected callback!");

                Console.Write("Sending message...");
                ConsoleMessage message = new ConsoleMessage { Text = "New Client!" };
                peer.Send(message);
                Console.WriteLine("Message Sent!");
            };

            string cmd = string.Empty;

            while (cmd != "q")
            {
                cmd = Console.ReadLine();
                peer.Send(new ConsoleMessage { Text = cmd });
            }

            Console.ReadLine();
            peer.Disconnect();
        }
    }
}
