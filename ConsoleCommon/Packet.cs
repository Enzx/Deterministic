using MessagePack;

namespace ConsoleCommon
{
    [Union(0, typeof(ConsoleMessage))]
    [Union(1, typeof(TestPacket))]
    public  interface Packet
    {
      
    }

    public class TestPacket : Packet
    {
        [Key(0)]
        public int x = 1;
        public  byte[] Serialize()
        {
            return MessagePackSerializer.Serialize(this);
        }
    }
}