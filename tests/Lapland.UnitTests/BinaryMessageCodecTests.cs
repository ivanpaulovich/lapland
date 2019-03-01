namespace Lapland.UnitTests
{
    using Lapland.Core;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public sealed class BinaryMessageCodecTests
    {
        [Fact]
        public void EncodeDecode()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[10000];
            rnd.NextBytes(b);

            Message expectedMessage = new Message(
                new Dictionary<string, string>()
                {
                    { "Host", "google.com" }
                },
                b
            );

            BinaryMessageCodec codec = new BinaryMessageCodec();
            byte[] binaryData = codec.Encode(expectedMessage);
            Message actualMessage = codec.Decode(binaryData);

            Assert.NotNull(actualMessage);
            Assert.Equal(expectedMessage.Headers.Count, actualMessage.Headers.Count);

            foreach (var header in expectedMessage.Headers)
            {
                Assert.True(actualMessage.Headers.ContainsKey(header.Key));
                Assert.Equal(header.Value, actualMessage.Headers[header.Key]);
            }

            Assert.Equal(expectedMessage.Payload, actualMessage.Payload);
        }

        [Fact]
        public void EncodeDecode_WhenMultipleHeaders()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[10000];
            rnd.NextBytes(b);

            Message expectedMessage = new Message(
                new Dictionary<string, string>()
                {
                    { "Host", "google.com" },
                    { "MyHeader", "custom value" },
                    { "Test", "paulovich.net" }
                },
                b
            );

            BinaryMessageCodec codec = new BinaryMessageCodec();
            byte[] binaryData = codec.Encode(expectedMessage);
            Message actualMessage = codec.Decode(binaryData);

            Assert.NotNull(actualMessage);
            Assert.Equal(expectedMessage.Headers.Count, actualMessage.Headers.Count);

            foreach (var header in expectedMessage.Headers)
            {
                Assert.True(actualMessage.Headers.ContainsKey(header.Key));
                Assert.Equal(header.Value, actualMessage.Headers[header.Key]);
            }

            Assert.Equal(expectedMessage.Payload, actualMessage.Payload);
        }

        [Fact]
        public void EncodeDecode_WhenEmptyHeaders()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[10000];
            rnd.NextBytes(b);

            Message expectedMessage = new Message(
                new Dictionary<string, string>()
                {
                },
                b
            );

            BinaryMessageCodec codec = new BinaryMessageCodec();
            byte[] binaryData = codec.Encode(expectedMessage);
            Message actualMessage = codec.Decode(binaryData);

            Assert.NotNull(actualMessage);
            Assert.Equal(0, actualMessage.Headers.Count);
            Assert.Equal(expectedMessage.Headers.Count, actualMessage.Headers.Count);
            Assert.Equal(expectedMessage.Payload, actualMessage.Payload);
        }

        [Fact]
        public void EncodeDecode_ThrowsCodecException_WhenNullFields()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[10000];
            rnd.NextBytes(b);

            Message expectedMessage = new Message(
                null,
                null
            );

            BinaryMessageCodec codec = new BinaryMessageCodec();
            Assert.Throws<MessageCodecException>(() => codec.Encode(expectedMessage));
        }

        [Fact]
        public void EncodeDecode_ThrowsCodecException_WhenNullFields1()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[10000];
            rnd.NextBytes(b);

            Message expectedMessage = new Message(
                new Dictionary<string, string>()
                {
                },
                null
            );

            BinaryMessageCodec codec = new BinaryMessageCodec();
            Assert.Throws<MessageCodecException>(() => codec.Encode(expectedMessage));
        }

        [Fact]
        public void EncodeDecode_ThrowsCodecException_WhenNullFields2()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[10000];
            rnd.NextBytes(b);

            Message expectedMessage = new Message(
                new Dictionary<string, string>()
                {
                    { "MyHeader", null }
                },
                null
            );

            BinaryMessageCodec codec = new BinaryMessageCodec();
            Assert.Throws<MessageCodecException>(() => codec.Encode(expectedMessage));
        }

        [Fact]
        public void EncodeDecode_ThrowsCodecException_WhenPayloadTooBig()
        {
            Random rnd = new Random();
            Byte[] b = new Byte[100000000];
            rnd.NextBytes(b);

            Message expectedMessage = new Message(
                new Dictionary<string, string>()
                {
                    { "MyHeader", "Test" }
                },
                b
            );

            BinaryMessageCodec codec = new BinaryMessageCodec();
            Assert.Throws<MessageCodecException>(() => codec.Encode(expectedMessage));
        }
    }
}