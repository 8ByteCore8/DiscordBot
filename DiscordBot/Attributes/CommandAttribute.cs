using System;

namespace DiscordBot.Attributes
{
    public class CommandAttribute : Attribute
    {
        public string Command { get; }
        public string Module { get; }
        public CommandType Type { get; }
    }
}