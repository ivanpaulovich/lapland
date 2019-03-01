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
            if (headers == null)
            {
                throw new MessageCodecException("Headers is null. Please provide.");
            }

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
            if (text == null)
            {
                throw new MessageCodecException("Text is null. Please provide.");
            }

            // TODO: implement the GetBytes method then remove the dependency on System.IO
            byte[] sourceSerializedText = Encoding.UTF8.GetBytes(text);
            if (sourceSerializedText.Length > MaximumHeaderNameLength)
                throw new MessageCodecException($"Header data is longer that { MaximumHeaderNameLength }. Please provide a valid data.");

            byte[] boundedBuffer = new byte[MaximumHeaderNameLength];

            int length = Math.Min(
                MaximumHeaderNameLength,
                sourceSerializedText.Length);

            SerializeInteger(buffer, length);
            buffer.Write(sourceSerializedText, 0, length);
        }

        private void SerializePayload(MemoryStream buffer, byte[] payload)
        {
            if (payload == null)
            {
                throw new MessageCodecException("Payload is null. Please provide.");
            }

            SerializeInteger(buffer, payload.Length);
            buffer.Write(payload, 0, payload.Length);
        }
    }
}