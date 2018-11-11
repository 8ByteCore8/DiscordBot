using DiscordBot.Attributes;
using DiscordBot.Common;
using DiscordBot.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DiscordBot
{
    /// <summary>
    /// Контролер по умолчанию.
    /// </summary>
    internal class DefaultControler : ICommandManager
    {
        private Type[] Commands { get; set; }

        public void Dispose()
        {
            Commands = null;
            GC.Collect();
        }

        public IEnumerable<ICommand> FindAllCommand(string name)
        {
            List<ICommand> commands = new List<ICommand>();

            foreach (Type type in Commands)
                if (type.GetCustomAttribute<CommandAttribute>().Command == name)
                    commands.Add((ICommand)Activator.CreateInstance(type));
            return commands;
        }

        public ICommand FindCommand(string module, string name)
        {
            foreach (Type type in Commands)
            {
                CommandAttribute attribute = type.GetCustomAttribute<CommandAttribute>();
                if (attribute.Command == name && attribute.Module == module)
                    return (ICommand)Activator.CreateInstance(type);
            }
            return null;
        }

        public ICommand FindCommand(string name)
        {
            foreach (Type type in Commands)
                if (type.GetCustomAttribute<CommandAttribute>().Command == name)
                    return (ICommand)Activator.CreateInstance(type);

            return null;
        }

        public void LoadCommands() => ReloadCommands();

        public ICommand Parse(string commandText)
        {
            Regex regex = new Regex(@"(\S+)\s(\S+)((?:\s\S+)*)");
            if (regex.IsMatch(commandText))
                throw new Exceptions.CommandFormatException("Неверный формат команды");

            GroupCollection groups = regex.Match(commandText).Groups;

            ICommand command = FindCommand(groups[1].Value, groups[2].Value);

            if (command == null)
                throw new CommandNotFoundException("Введена неизвестная команда");

            command.Args = groups[3].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return command;
        }

        public void ReloadCommands()
        {
            throw new System.NotImplementedException();
        }
    }
}