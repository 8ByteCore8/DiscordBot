using System;
using System.Threading.Tasks;

namespace DiscordBot.Common
{
    public interface ILoger : IDisposable
    {
        void Init();

        void Log(string message);

        Task LogAsync(string message);
    }
}