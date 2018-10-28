using System;
using DSharpPlus;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string token;

            if (args[0] != null)
                token = args[0];
            else
            {
                Console.WriteLine("Введите токен для подключения:");
                token = Console.ReadLine();
            }

            Console.WriteLine($"Бот будет запущен по токену \" {token} \"");

            DiscordClient discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot
            });
        }
    }
}
