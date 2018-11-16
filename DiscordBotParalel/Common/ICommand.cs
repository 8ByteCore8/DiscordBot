using Discord.WebSocket;
using System;

namespace DiscordBot.Common
{
    public interface ICommand : IDisposable
    {
        string[] Args { get; set; }

        void ExecuteAsBot(SocketMessage message);

        void ExecuteAsConsole(DiscordBot bot);
    }
}