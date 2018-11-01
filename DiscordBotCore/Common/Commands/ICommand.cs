namespace DiscordBotCore.Common.Commands
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
        void Execute();
    }
}
