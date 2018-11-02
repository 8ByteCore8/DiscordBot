using System;

namespace DiscordBot.Common.Commands
{
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string command, string module, CommandType type)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Module = module ?? throw new ArgumentNullException(nameof(module));
            Type = type;
        }

        /// <summary>
        /// Имя каманды.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Модуль к которому относиться эта команда.
        /// </summary>
        public string Module { get; }

        /// <summary>
        /// Определяет тип команды.
        /// </summary>
        public CommandType Type { get; }
    }
}
