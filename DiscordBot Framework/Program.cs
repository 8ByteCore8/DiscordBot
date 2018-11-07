using System;

namespace DiscordBot_Framework
{
    class Program
    {
        static void Main(string[] args)
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

            using (DiscordBot.DiscordBot bot = new DiscordBot.DiscordBot(token))
            {
                while (true)
                {
                    string commant = Console.ReadLine().ToLower().Trim();

                    if (commant == "exit")
                        break;
                    else
                        bot.Execute(commant);
                }
            }
        }
    }
}
