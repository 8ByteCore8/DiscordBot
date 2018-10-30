using System;

namespace DiscordBotCore.Common.Commands
{
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string name, CommandType type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }

        public string Name { get; }
        public CommandType Type { get; }
    }
}
