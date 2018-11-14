using System;

namespace DiscordBot.Attributes
{
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string name, CommandType type, string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }

        public string Description { get; }
        public string Name { get; }
        public CommandType Type { get; }
    }
}