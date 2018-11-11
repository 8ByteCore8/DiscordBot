using DSharpPlus.EventArgs;
using System;

namespace DiscordBot.Common
{
    public interface ICommand : IDisposable
    {
        string[] Args { get; set; }

        void ExecuteAsBot(MessageCreateEventArgs message);

        void ExecuteAsConsole(DiscordBot bot);
    }
}