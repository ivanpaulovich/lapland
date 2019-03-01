namespace Lapland.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public sealed class BinaryMessageCodec : IMessageCodec
    {
        private BinaryMessageEncoder encoder;
        private BinaryMessageDecoder decoder;

        public BinaryMessageCodec()
        {
            encoder = new BinaryMessageEncoder();
            decoder = new BinaryMessageDecoder();
        }

        public byte[] Encode(Message message)
        {
            return encoder.Encode(message);
        }

        public Message Decode(byte[] data)
        {
            return decoder.Decode(data);
        }
    }
}