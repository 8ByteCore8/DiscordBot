using System;
using System.Runtime.Serialization;

namespace DiscordBotCore.Exceptions
{
    class ControlerException : Exception
    {
        public ControlerException()
        {
        }

        public ControlerException(string message) : base(message)
        {
        }

        public ControlerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ControlerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
