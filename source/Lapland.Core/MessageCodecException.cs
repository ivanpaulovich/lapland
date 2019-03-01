namespace Lapland.Core
{
    using System;

    public sealed class MessageCodecException : Exception
    {
        public MessageCodecException(string message) : base(message)
        {
        }
    }
}