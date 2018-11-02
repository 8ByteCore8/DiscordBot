using System;

namespace DiscordBot.Common.Commands
{
    /// <summary>
    /// Определяет режим в котором будет исполняться команда.
    /// </summary>
    [Flags]
    public enum CommandType : byte
    {
        /// <summary>
        /// Режим бота.
        /// </summary>
        Discord = 0b01,
        /// <summary>
        /// Консольный режим.
        /// </summary>
        Console = 0b10
    }
}
