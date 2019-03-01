namespace Lapland.Core
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class Message
    {
        public Dictionary<string, string> Headers { get; set; }
        public byte[] Payload { get; set; }
        
    }
}