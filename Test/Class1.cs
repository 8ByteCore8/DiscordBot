using DiscordBotCore.Common;
using DiscordBotCore.Common.Commands;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;

namespace Test
{
    [Command("test", "basic", CommandType.Discord | CommandType.Console)]
    public class TestCommand : ICommand
    {
        public string[] Args { get; set; }

        public void ExecuteAsBot(MessageCreateEventArgs message)
        {
            message.Message.RespondAsync("Я тестовая комманда для бота");
        }

        public void ExecuteAsConsole(DiscordClient client)
        {
            Console.WriteLine("Я тестовая комманда для консоли");
        }
    }
}
