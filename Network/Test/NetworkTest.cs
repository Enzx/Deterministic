using System.Net;
using ConsoleCommon;
using NUnit.Framework;

namespace Deterministic.Network.Test
{
    internal class NetworkTest
    {

        //private class TestPacket : Packet
        //{
        //    public override byte[] Serialize<T>()
        //    {
        //        return null;
        //    }

           
        //}

        [Test]
        public void CreateTcpPeer()
        {
            TcpPeer tcpPeer = new TcpPeer();

        }

        [Test]
        public void TestConnect()
        {
            TcpPeer tcpPeer = new TcpPeer();
            tcpPeer.Connect(IPAddress.Parse("192.168.1.1"), 80);
          //  tcpPeer.Send(new TestPacket());
        }
    }
}
