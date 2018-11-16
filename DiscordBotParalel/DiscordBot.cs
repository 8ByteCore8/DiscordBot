using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Attributes;
using DiscordBot.Common;
using DiscordBot.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    /// <summary>
    /// Основной класс бота.
    /// </summary>
    public class DiscordBot : IDisposable
    {
        /// <summary>
        /// Создаёт новый обьект бота.
        /// </summary>
        /// <param name="token">Токен для подключения.</param>
        /// <param name="loger">Пользовательский класс для логирования.</param>
        public DiscordBot(string token, ILoger loger = null, ICommandManager controler = null)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (loger == null)
                Loger = new DefaultLoger();
            else
                Loger = loger;

            if (controler == null)
                CommandManager = new DefaultManager();
            else
                CommandManager = controler;

            CommandManager.Sturtup();

            CommandManager.LoadCommands();

            //инициализация ботa

            DiscordClient = new DiscordSocketClient();

            CommandService command = new CommandService();

            IServiceProvider provider = new ServiceCollection()
                .AddSingleton(DiscordClient)
                .AddSingleton(command)
                .BuildServiceProvider();

            DiscordClient.LoginAsync(TokenType.Bot, token);

            #region Обработчики событий

            DiscordClient.MessageReceived += DiscordClient_MessageReceived;
            DiscordClient.MessageUpdated += DiscordClient_MessageUpdated; ;

            #endregion Обработчики событий

            //подключение бота к серверу
            try
            {
                DiscordClient.StartAsync().Wait();
                Mention = DiscordClient.CurrentUser.Mention;
            }
            catch (Exception ex)
            {
                throw new BotException($"Не удалось подключиться к дискорду.\n\tподробности: {ex.Message}", ex);
            }

            Loger.Log($"Бот успешно подключён");
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~DiscordBot()
        {
            Dispose();
        }

        public ICommandManager CommandManager { get; set; }

        /// <summary>
        /// Обект бота.
        /// </summary>
        public DiscordSocketClient DiscordClient { get; }

        /// <summary>
        /// Логер текущего бота.
        /// </summary>
        public ILoger Loger { get; set; }

        private string Mention { get; set; }

        /// <summary>
        /// Метод для коректного завершения работы бота и освобождения ресурсов.
        /// </summary>
        public void Dispose()
        {
            if (DiscordClient != null)
            {
                DiscordClient.StopAsync().Wait();
                DiscordClient.LogoutAsync().Wait();
                DiscordClient.Dispose();
            }

            if (Loger != null)
                Loger.Dispose();

            GC.Collect();
        }

        /// <summary>
        /// Метод выполняющий команду в консольном режиме.
        /// </summary>
        /// <param name="text">Текст команды.</param>
        public void Execute(string text)
        {
            try
            {
                using (ICommand command = CommandManager.Parse(text.Trim(), CommandType.Console))
                    command.ExecuteAsConsole(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Коректно завершает работу бота с последующим закрытием программы.
        /// </summary>
        public void Stop()
        {
            Dispose();
            Environment.Exit(0);
        }

        private Task DiscordClient_MessageReceived(SocketMessage arg)
        {
            return Task.Factory.StartNew(() =>
            {
                string text = arg.Content.Trim();
                if (arg.Author.IsBot)
                    return;

                if (text.ToLower().StartsWith("!bot "))
                    text = text.Replace("!bot ", "");
                else if (text.StartsWith(Mention))
                    text = text.Replace(Mention, "").Trim();
                else
                    return;

                try
                {
                    using (ICommand command = CommandManager.Parse(text, CommandType.Discord))
                        command.ExecuteAsBot(arg);
                }
                catch (Exception ex)
                {
                    arg.Channel.SendMessageAsync(ex.Message);
                }
            });
        }

        private Task DiscordClient_MessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
        {
            return Task.Factory.StartNew(() =>
            {
                if (DateTimeOffset.Now - arg2.CreatedAt > new TimeSpan(1, 0, 0))
                    return;

                string text = arg2.Content.Trim();
                if (arg2.Author.IsBot)
                    return;

                if (text.ToLower().StartsWith("!bot "))
                    text = text.Replace("!bot ", "");
                else if (text.StartsWith(Mention))
                    text = text.Replace(Mention, "").Trim();
                else
                    return;

                try
                {
                    using (ICommand command = CommandManager.Parse(text, CommandType.Discord))
                        command.ExecuteAsBot(arg2);
                }
                catch (Exception ex)
                {
                    arg2.Channel.SendMessageAsync(ex.Message);
                }
            });
        }
    }
}