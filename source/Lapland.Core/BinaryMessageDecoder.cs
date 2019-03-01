namespace Lapland.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public sealed class BinaryMessageDecoder
    {
        public Message Decode(byte[] data)
        {
            Dictionary<string, string> headers;
            byte[] payload;

            using(MemoryStream buffer = new MemoryStream(data))
            {
                using(BinaryReader reader = new BinaryReader(buffer))
                {
                    headers = ReadHeaders(reader);
                    payload = ReadPayload(reader);
                }
            }

            Message message = new Message(headers, payload);
            return message;
        }

        private Dictionary<string, string> ReadHeaders(BinaryReader reader)
        {
            var headers = new Dictionary<string, string>();
            int headersCount = reader.ReadInt32();

            for (int i = 0; i < headersCount; i++)
            {
                var headerItem = ReadHeaderItem(reader);
                headers.Add(headerItem.Item1, headerItem.Item2); 
            }

            return headers;
        }

        private Tuple<string, string> ReadHeaderItem(BinaryReader reader)
        {
            var name = ReadText(reader);
            var value = ReadText(reader);
            var headerItem = new Tuple<string, string>(name, value);
            return headerItem;
        }

        private string ReadText(BinaryReader reader)
        {
            var length = reader.ReadInt32();
            var textArray = reader.ReadBytes(length);
            var text = System.Text.Encoding.UTF8.GetString(textArray);
            return text;
        }

        private byte[] ReadPayload(BinaryReader reader)
        {
            var length = reader.ReadInt32();
            var payload = reader.ReadBytes(length);
            return payload;
        }
    }
}