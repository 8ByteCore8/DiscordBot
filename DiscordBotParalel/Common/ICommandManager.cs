using DiscordBot.Attributes;
using System;

namespace DiscordBot.Common
{
    public interface ICommandManager : IDisposable
    {
        ICommand FindCommand(string commandName, CommandType type);

        void LoadCommands();

        ICommand Parse(string text, CommandType type);

        void ReloadCommands();

        void Sturtup();
    }
}