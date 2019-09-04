using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConsoleCommon;
using Deterministic.Network;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpPeer peer = new TcpPeer();
            peer.Connect(IPAddress.Loopback, 50050);
            peer.OnConnected += () =>
            {
                Console.WriteLine("Connected callback!");

                Console.Write("Sending message...");
                Task.Delay(1000);
                ConsoleMessage message = new ConsoleMessage();
                message.Text = "Hello, World!";
                peer.Send(message);
                Console.WriteLine("Message Sent!");
            };
           
            Console.ReadLine();
            peer.Disconnect();
        }
    }
}
