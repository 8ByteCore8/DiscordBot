using System;
using System.Runtime.Serialization;

namespace DiscordBot.Exceptions
{
    internal class CommandFormatException : Exception
    {
        public CommandFormatException()
        {
        }

        public CommandFormatException(string message) : base(message)
        {
        }

        public CommandFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommandFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}