namespace Lapland.UnitTests
{
    using Lapland.Core;
    using System.Collections.Generic;
    using Xunit;

    public sealed class BinaryMessageEncoderTests
    {
        [Fact]
        public void Encode_ReturnsByteArray_WhenSingleHeaderAndEmptyPayload()
        {
            Message singleHeaderMessage = new Message()
            {
                Headers = new Dictionary<string, string>()
                {
                    { "Host", "google.com" }
                },
                Payload = new byte[0]
            };

            BinaryMessageEncoder sut = new BinaryMessageEncoder();
            byte[] actual = sut.Encode(singleHeaderMessage);

            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
        }
    }
}