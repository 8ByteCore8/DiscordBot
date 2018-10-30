using System.Threading.Tasks;
using System;

namespace DiscordBotCore.Common
{
    public interface ILoger : IDisposable
    {
        void Log(string LogText);
        Task LogAsync(string LogText);
    }
}
