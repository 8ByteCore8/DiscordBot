using DiscordBot.Attributes;
using DiscordBot.Common;
using DiscordBot.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

        public ICommand FindCommand(string commandName, CommandType type)
        {
            foreach (Type command in Commands)
            {
                CommandAttribute attribute = command.GetCustomAttribute<CommandAttribute>();
                if (attribute.Name == commandName && attribute.Type.HasFlag(type))
                    return (ICommand)Activator.CreateInstance(command);
            }
            return null;
        }

        public void LoadCommands() => ReloadCommands();

        public ICommand Parse(string text, CommandType type)
        {
            List<string> parts = new List<string>();
            parts.AddRange(text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            if (parts.Count == 0)
                throw new CommandFormatException("Неверный формат команды");

            ICommand command = FindCommand(parts[0], type);

            if (command == null)
                throw new CommandNotFoundException("Введена неизвестная команда");

            parts.RemoveAt(0);

            command.Args = parts.ToArray();

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