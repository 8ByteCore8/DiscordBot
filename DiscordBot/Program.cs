using System;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = null;

            foreach (string arg in args)
            {
                if (arg.StartsWith("token="))
                {
                    arg.Replace("token=", "");
                    token = arg;
                }
            }

            if (token == null)
            {
                Console.WriteLine("Введите токен для бота:");
                token = Console.ReadLine();
            }

            try
            {
                DiscordBotCore.DiscordBot bot = new DiscordBotCore.DiscordBot(token);

                while (true)
                {
                    string command = Console.ReadLine().ToLower().Trim();

                    if (command == "exit")
                    {
                        bot.Dispose();
                        break;
                    }
                    else
                    {
                        bot.Execute(command);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
