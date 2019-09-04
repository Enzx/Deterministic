using MessagePack;

namespace ConsoleCommon
{
    [MessagePackObject]
    public class ConsoleMessage : Packet
    {
        [Key(0)]
        public string Text;

      

       
    }
}
