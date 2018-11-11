using System;

namespace DiscordBot.Common
{
    public interface ICommand : IDisposable
    {
        string[] Args { get; set; }

        void ExecuteAsBot(DSharpPlus.EventArgs.MessageCreateEventArgs message);

        void ExecuteAsConsole(DiscordBot bot);
    }
}