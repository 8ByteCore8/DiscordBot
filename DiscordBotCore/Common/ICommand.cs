using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DiscordBotCore.Common
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
        /// Действие команды.
        /// </summary>
        void ExecuteAsConsole(DiscordClient client);

        void ExecuteAsBot(MessageCreateEventArgs message);
    }
}
