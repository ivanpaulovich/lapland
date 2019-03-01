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
            using(MemoryStream buffer = new MemoryStream())
            {
                SerializeHeaders(buffer, message.Headers);
                SerializePayload(buffer, message.Payload);
                
                byte[] output = buffer.ToArray();
                return output;
            }
        }

        private void SerializeHeaders(MemoryStream buffer, Dictionary<string, string> headers)
        {
            SerializeInteger(buffer, headers.Count);
            
            foreach (var header in headers)
                SerializeHeaderItem(buffer, header);
        }

        private void SerializeInteger(MemoryStream buffer, int length)
        {
            byte[] serializedInt = BitConverter.GetBytes(length);
            buffer.Write(serializedInt, 0, serializedInt.Length);
        }

        private void SerializeHeaderItem(MemoryStream buffer, KeyValuePair<string, string> header)
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

            SerializeInteger(buffer, length);

            Array.Copy(sourceSerializedText, boundedBuffer, length);
            buffer.Write(boundedBuffer, 0, MaximumHeaderNameLength);
        }

        private void SerializePayload(MemoryStream buffer, byte[] payload)
        {
            buffer.Write(payload, 0, payload.Length);
        }
    }
}