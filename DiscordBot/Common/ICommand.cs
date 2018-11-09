using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DiscordBot.Common
{
    /// <summary>
    /// Команда которую исполняет бот.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Аргументы команды.
        /// </summary>
        string[] Args { get; set; }

        /// <summary>
        /// Выполняет команду в режиме бота.
        /// </summary>
        void ExecuteAsBot(MessageCreateEventArgs message);

        /// <summary>
        /// Выполняет команду в консольном режиме.
        /// </summary>
        void ExecuteAsConsole(DiscordClient client);
    }
}