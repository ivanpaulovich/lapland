namespace Lapland.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public sealed class BinaryMessageEncoder
    {
        private const int MaximumHeaderNameLength = 1023;
        
        public byte[] Encode(Message message)
        {
            MemoryStream buffer = new MemoryStream();

            foreach (var header in message.Headers)
                SerializeHeader(buffer, header);

            SerializePayload(buffer, message.Payload);
            
            byte[] output = buffer.ToArray();
            return output;
        }

        private void SerializeHeader(MemoryStream buffer, KeyValuePair<string, string> header)
        {
            SerializeString(buffer, header.Key);
            SerializeString(buffer, header.Value);
        }

        private void SerializeString(MemoryStream buffer, string text)
        {
            // TODO: implement the GetBytes method then remove the dependency on System.IO
            byte[] sourceSerializedText = Encoding.UTF8.GetBytes(text);
            if (sourceSerializedText.Length > MaximumHeaderNameLength)
                throw new MessageCodecException();

            byte[] boundedBuffer = new byte[MaximumHeaderNameLength];

            int length = Math.Min(
                MaximumHeaderNameLength,
                sourceSerializedText.Length);

            Array.Copy(sourceSerializedText, boundedBuffer, length);

            buffer.Write(boundedBuffer, 0, MaximumHeaderNameLength);
        }

        private void SerializePayload(MemoryStream buffer, byte[] payload)
        {
            buffer.Write(payload, 0, payload.Length);
        }
    }
}