using System;

namespace DiscordBot.Attributes
{
    [Flags]
    public enum CommandType
    {
        Discord = 1,
        Console = 2
    }
}