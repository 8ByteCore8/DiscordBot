using System;
using DSharpPlus;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot
{
    class Program
    {
        static Loger Loger { get; set; } = new Loger($"Logs\\{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()}.log");
        static void Main(string[] args)
        {
            string token;

            if (args[0] != null)
                token = args[0];
            else
            {
                Console.WriteLine("Введите токен для бота:");
                token = Console.ReadLine();
            }

            Loger.Log($"Бот будет запущен по токену \"{token}\".");

            DiscordClient discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += Discord_MessageCreated;

            discord.ConnectAsync().Wait();

            Loger.Log($"Бот успешно подключён.");

            while (true)
            {
                string command = Console.ReadLine().ToLower().Trim();

                if (command == "exit")
                    break;
            }
        }

        private static Task Discord_MessageCreated(DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            return Task.Factory.StartNew(() =>
            {
                //зачем терять время на неонносясиеся к нам сообщения?
                if (!e.Message.Content.StartsWith("#"))
                    return;

                //пользовательское сообщение
                DiscordMessage userMessage = e.Message;
                //содержимое ответа
                string messageText = $"{e.Message.Author.Mention} ";


                #region Опредиление запроса
                if (userMessage.Content.ToLower().StartsWith("#time"))
                    messageText += $"{DateTime.Now.ToShortTimeString()}";

                if (userMessage.Content.ToLower().StartsWith("#date"))
                    messageText += $"{DateTime.Now.ToShortDateString()}";

                if (userMessage.Content.ToLower().StartsWith("#code sort"))
                {
                    messageText += "```\n" +
                    "public static void sorting(double[] arr, long first, long last)\n" +
                    "{\n" +
                        "\tdouble p = arr[(last - first) / 2 + first];\n" +
                        "\tdouble temp;\n" +
                        "\tlong i = first, j = last;\n" +
                        "\twhile (i <= j)\n" +
                        "\t{\n" +
                            "\t\twhile (arr[i] < p && i <= last) ++i;\n" +
                            "\t\twhile (arr[j] > p && j >= first) --j;\n" +
                            "\t\tif (i <= j)\n" +
                            "\t\t{" +
                                "\t\t\ttemp = arr[i];\n" +
                                "\t\t\tarr[i] = arr[j];\n" +
                                "\t\t\tarr[j] = temp;\n" +
                                "\t\t\t++i; --j;\n" +
                            "\t\t}\n" +
                        "\t}\n" +
                        "\tif (j > first) sorting(arr, first, j);\n" +
                        "\tif (i < last) sorting(arr, i, last);\n" +
                    "}\n" +
                    "```";
                }
                #endregion



                //чтобы чат не засорять
                userMessage.DeleteAsync();
                //отправка ответа
                DiscordMessage respondMessage = e.Message.RespondAsync(messageText).Result;
                ////10 секунд на чтение
                //Thread.Sleep(60000);
                ////чтобы чат не засорять ботовским спамом
                //respondMessage.DeleteAsync();
            });
        }
    }
}
