using System;
using System.Runtime.Serialization;

namespace DiscordBotCore.Exceptions
{
    class InvalidCommandException : Exception
    {
        public InvalidCommandException()
        {
        }

        public InvalidCommandException(string message) : base(message)
        {
        }

        public InvalidCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
