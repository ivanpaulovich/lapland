namespace Lapland.UnitTests
{
    using Lapland.Core;
    using System.Collections.Generic;
    using Xunit;

    public sealed class BinaryMessageDecoderTests
    {
        [Fact]
        public void Decode_ReturnsMessage_WhenByArray()
        {
            Message expectedMessage = new Message(
                new Dictionary<string, string>()
                {
                    { "Host", "google.com" }
                },
                new byte[0]
            );

            BinaryMessageEncoder encoder = new BinaryMessageEncoder();
            byte[] serializedMessage = encoder.Encode(expectedMessage);

            BinaryMessageDecoder decoder = new BinaryMessageDecoder();
            Message actualMessage = decoder.Decode(serializedMessage);

            Assert.NotNull(actualMessage);
        }
    }
}