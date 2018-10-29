using System;
using System.IO;
using System.Threading.Tasks;
using DiscordBotCore.Common;

namespace DiscordBot
{
    class Loger : IDisposable, DiscordBotCore.Common.ILoger
    {
        public Loger(string Path)
        {
            File = new FileStream(Path, FileMode.Create);
            LogWriter = new StreamWriter(File);
        }

        FileStream File { get; }
        StreamWriter LogWriter { get; }

        public void Log(string LogText)
        {
            DateTime time = DateTime.Now;
            string log = $"[{time.ToShortDateString()} | {time.ToShortTimeString()}] --- {LogText}";
            Console.WriteLine(log);
            LogWriter.WriteLine(log);
        }

        public Task LogAsync(string LogText)
        {
            return Task.Factory.StartNew(() => {
                DateTime time = DateTime.Now;
                string log = $"[{time.ToShortDateString()} | {time.ToShortTimeString()}] --- {LogText}";
                Console.WriteLine(log);
                LogWriter.WriteLine(log);
            });
        }

        public void Dispose()
        {
            LogWriter.Dispose();
            File.Dispose();
        }

        ~Loger()
        {
            Dispose();
        }
    }
}
