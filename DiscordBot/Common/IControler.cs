using DiscordBot.Common.Commands;

namespace DiscordBot.Common
{
    /// <summary>
    /// Класс определяющий команду.
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
