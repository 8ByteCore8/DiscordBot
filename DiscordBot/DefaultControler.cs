using DiscordBot.Attributes;
using DiscordBot.Common;
using DiscordBot.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DiscordBot
{
    /// <summary>
    /// Контролер по умолчанию.
    /// </summary>
    internal class DefaultControler : ICommandManager
    {
        private Type[] _commands;

        private Assembly[] _modules;

        public DefaultControler() => AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

        private Type[] Commands
        {
            get => _commands;
            set
            {
                _commands = value;
                Console.WriteLine($"Загружено команд: {_commands.Length}.");
            }
        }

        private Assembly[] Modules
        {
            get => _modules;
            set
            {
                _modules = value;
                Console.WriteLine($"Загружено модулей: {_modules.Length}.");
            }
        }

        private string ModulesPath { get; } = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Modules";

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

        public ICommand FindCommand(string module, string name, CommandType type)
        {
            foreach (Type temp in Commands)
            {
                CommandAttribute attribute = temp.GetCustomAttribute<CommandAttribute>();
                if (attribute.Command == name && attribute.Module == module && attribute.Type.HasFlag(type))
                    return (ICommand)Activator.CreateInstance(temp);
            }
            return null;
        }

        public void LoadCommands() => ReloadCommands();

        public ICommand Parse(string commandText, CommandType type)
        {
            Regex regex = new Regex(@"^(\S+)\s(\S+)((?:\s\S+)*)$");
            if (!regex.IsMatch(commandText))
                throw new CommandFormatException("Неверный формат команды");

            GroupCollection groups = regex.Match(commandText).Groups;

            ICommand command = FindCommand(groups[1].Value, groups[2].Value, type);

            if (command == null)
                throw new CommandNotFoundException("Введена неизвестная команда");

            command.Args = groups[3].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return command;
        }

        public void ReloadCommands()
        {
            IEnumerable<string> modules = Directory.GetFiles(ModulesPath).Where(x => Path.GetExtension(x) == ".dll");

            List<Type> commands = new List<Type>();

            foreach (Assembly module in Modules)
            {
                Type[] types = module.GetTypes();

                foreach (Type type in types)
                    if (typeof(ICommand).IsAssignableFrom(type) && type.IsClass)
                        commands.Add(type);
            }

            Commands = commands.ToArray();
        }

        public void Sturtup()
        {
            if (!Directory.Exists(ModulesPath))
                Directory.CreateDirectory(ModulesPath);

            IEnumerable<string> modulesPath = Directory.GetFiles(ModulesPath).Where(x => Path.GetExtension(x) == ".dll");

            List<Assembly> modules = new List<Assembly>();

            foreach (string module in modulesPath)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(module);

                    Type[] types = assembly.GetTypes();

                    foreach (Type type in types)
                        if (typeof(IStartup).IsAssignableFrom(type) && type.IsClass)
                            ((IStartup)Activator.CreateInstance(type)).Sturtup();

                    modules.Add(assembly);
                }
                catch { }
            }

            Modules = modules.ToArray();
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) => Modules.FirstOrDefault(x => x.FullName == args.Name);
    }
}