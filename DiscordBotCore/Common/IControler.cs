using DiscordBotCore.Common.Commands;
using System.Threading.Tasks;

namespace DiscordBotCore.Common
{
    /// <summary>
    /// Класс определяющий команду и исполняющий её.
    /// </summary>
    interface IControler
    {
        /// <summary>
        /// Текущая команда.
        /// </summary>
        ICommand Command { get; }


        /// <summary>
        /// Разбор текста в команду.
        /// </summary>
        /// <param name="text">Текст команды.</param>
        void Parse(string text);

        /// <summary>
        /// Запускает текущюю команду.
        /// </summary>
        void Execute();

        /// <summary>
        /// Запускает текущюю команду в асинхронном режиме.
        /// </summary>
        Task ExecuteAsync();
    }
}
