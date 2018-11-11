using System;

namespace DiscordBot.Attributes
{
    [Flags]
    public enum CommandType
    {
        Discord,
        Console
    }
}