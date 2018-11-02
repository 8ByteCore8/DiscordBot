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
            Console.WriteLine($"Комманд {Commands.Length}");

            Regex regex = new Regex(@"^(\S+)\s(\S+)((?:\s\S+)*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (!regex.IsMatch(text))
                throw new ControlerException($"Комманда {text} имела неверный формат.");

            GroupCollection groupCollection = regex.Match(text).Groups;

            string module = groupCollection[1].Value;
            string commandStr = groupCollection[2].Value;
            string[] args = groupCollection[3].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ICommand command;
            try
            {
                Console.WriteLine(module);
                Console.WriteLine(commandStr);

                command = (ICommand)Activator.CreateInstance(Commands.FirstOrDefault(x =>
                {
                    CommandAttribute attribute = x.GetCustomAttribute<CommandAttribute>();
                    Console.WriteLine(attribute.Module);
                    Console.WriteLine(attribute.Command);
                    Console.WriteLine(attribute.Type.HasFlag(type));

                    return attribute.Module == module && attribute.Command == commandStr && attribute.Type.HasFlag(type);
                }));

                command.Args = args;

                return command;
            }
            catch
            {
                throw new ControlerException("Введена неизвестная команда.");
            }
        }
    }
}
