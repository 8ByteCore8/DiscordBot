using DiscordBot.Attributes;
using System;
using System.Collections.Generic;

namespace DiscordBot.Common
{
    public interface ICommandManager : IDisposable
    {
        IEnumerable<ICommand> FindAllCommand(string name);

        ICommand FindCommand(string name);

        ICommand FindCommand(string module, string name);

        ICommand FindCommand(string module, string name, CommandType type);

        void LoadCommands();

        ICommand Parse(string commandText, CommandType type);

        void ReloadCommands();
    }
}