using DSharpPlus.Entities;
using System;

namespace DiscordBot.Common
{
    public interface ICommand : IDisposable
    {
        string[] Args { get; set; }

        void ExecuteAsBot(DiscordMessage message);

        void ExecuteAsConsole(DiscordBot bot);
    }
}