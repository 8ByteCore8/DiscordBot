﻿using DiscordBotCore.Common.Commands;

namespace DiscordBotCore.Common
{
    /// <summary>
    /// Класс определяющий команду и исполняющий её.
    /// </summary>
    public interface IControler
    {
        /// <summary>
        /// Разбор текста в команду.
        /// </summary>
        /// <param name="text">Текст команды.</param>
        ICommand Parse(string text, CommandType type);
    }
}
