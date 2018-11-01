using DiscordBotCore.Common;
using DiscordBotCore.Common.Commands;
using DiscordBotCore.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DiscordBotCore
{
    class DefaultControler : IControler
    {
        static DefaultControler()
        {
            string dirPath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Modules";

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            LoadCommands();
        }

        public static Type[] Commands { get; private set; }

        public static void LoadCommands()
        {
            List<Type> commands = new List<Type>();
            string dirPath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Modules";

            string[] files = Directory.GetFiles(dirPath);
            foreach (string file in files)
            {
                try
                {
                    Type[] types = Assembly.LoadFile(file).GetExportedTypes();
                    foreach (Type type in types)
                    {
                        if (typeof(ICommand).IsAssignableFrom(type) && type.IsClass)
                            commands.Add(type);
                    }
                }
                catch { }
            }

            Commands = commands.ToArray();
        }

        public ICommand Parse(string text, CommandType type)
        {
            Regex regex = new Regex(@"^(\S+)\s(\S+)((?:\s\S+)*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (!regex.IsMatch(text))
                throw new ControlerException($"Комманда {text} имела неверный формат.");

            GroupCollection groupCollection = regex.Match(text).Groups;

            string module = groupCollection[0].Value;
            string commandStr = groupCollection[1].Value;
            string[] args = groupCollection[2].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ICommand command;
            try
            {
                command = (ICommand)Activator.CreateInstance(Commands.FirstOrDefault(x =>
                {
                    CommandAttribute attribute = x.GetCustomAttribute<CommandAttribute>();
                    return attribute.Module == module && attribute.Command == commandStr && attribute.Type.HasFlag(type);
                }));

                command.Args = args;
            }
            catch
            {
                throw new ControlerException("Введена неизвестная команда.");
            }

            return command;
        }
    }
}
