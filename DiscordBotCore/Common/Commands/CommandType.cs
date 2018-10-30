using System;

namespace DiscordBotCore.Common.Commands
{
    [Flags]
    public enum CommandType : byte
    {
        Discord = 0b01,
        Console = 0b10
    }
}
