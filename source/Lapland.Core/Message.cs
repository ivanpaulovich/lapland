namespace Lapland.Core
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class Message
    {
        public Message(Dictionary<string, string> headers, byte[] payload)
        {
            Headers = headers;
            Payload = payload;
        }

        //
        // Considering that Headers has unique values for each name
        //

        public Dictionary<string, string> Headers { get; }
        public byte[] Payload { get; }
    }
}