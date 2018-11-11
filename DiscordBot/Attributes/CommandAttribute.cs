using System;

namespace DiscordBot.Attributes
{
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string module, string command, CommandType type)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Type = type;
        }

        public string Command { get; }
        public string Module { get; }
        public CommandType Type { get; }
    }
}