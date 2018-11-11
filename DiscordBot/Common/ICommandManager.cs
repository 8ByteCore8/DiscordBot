using System;
using System.Collections.Generic;

namespace DiscordBot.Common
{
    public interface ICommandManager : IDisposable
    {
        IEnumerable<ICommand> FindAllCommand(string name);

        ICommand FindCommand(string name);

        ICommand FindCommand(string module, string name);

        void LoadCommands();

        ICommand Parse(string commandText);

        void ReloadCommands();
    }
}