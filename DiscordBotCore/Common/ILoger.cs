using System;
using System.Threading.Tasks;

namespace DiscordBotCore.Common
{
    /// <summary>
    /// Интерфейс логера
    /// </summary>
    public interface ILoger : IDisposable
    {
        /// <summary>
        /// Производит запись нового лога.
        /// </summary>
        /// <param name="LogText">Текст лога.</param>
        void Log(string LogText);

        /// <summary>
        /// Производит запись нового лога в аинхронном режиме.
        /// </summary>
        /// <param name="LogText">Текст лога.</param>
        Task LogAsync(string LogText);
    }
}
