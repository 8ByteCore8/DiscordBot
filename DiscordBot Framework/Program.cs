using System;

namespace DiscordBot_Framework
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string token = null;

            foreach (string arg in args)
            {
                if (arg.ToLower().Contains("token="))
                    token = arg.Replace("token=", "");
            }

            if (token == null)
            {
                Console.WriteLine("Введите токен для подключения:");
                token = Console.ReadLine();
            }

            try
            {
                using (DiscordBot.DiscordBot bot = new DiscordBot.DiscordBot(token) { HiPeople = "Всем привет! Я к вашим услугам!" })
                    while (true)
                        bot.Execute(Console.ReadLine().Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}