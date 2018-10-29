using System.Threading.Tasks;

namespace DiscordBotCore.Common
{
    public interface ILoger
    {
        void Dispose();
        void Log(string LogText);
        Task LogAsync(string LogText);
    }
}
