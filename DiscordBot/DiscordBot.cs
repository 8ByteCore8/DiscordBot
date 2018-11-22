using Discord;
using Discord.WebSocket;
using DiscordBot.Attributes;
using DiscordBot.Common;
using DiscordBot.Exceptions;
using System.Reflection;
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
        public DiscordBot(string token, ILoger loger = null, ICommandManager commandManager = null)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));



            Loger = loger;
            CommandManager = commandManager;



            if (Loger is null)
                Loger = new DefaultLoger();

            if (CommandManager is null)
                CommandManager = new DefaultManager();



            Init(token);
        }

        public DiscordBot(IBotConfigurations botConfigurations)
        {
            if (botConfigurations == null)
                throw new ArgumentNullException(nameof(botConfigurations));

            if (string.IsNullOrEmpty(botConfigurations.Token))
                throw new ArgumentNullException(nameof(botConfigurations.Token));



            Loger = botConfigurations.Loger;
            CommandManager = botConfigurations.CommandManager;
            HiPeople = botConfigurations.HiPeople;



            if (Loger is null)
                Loger = new DefaultLoger();

            if (CommandManager is null)
                CommandManager = new DefaultManager();



            Init(botConfigurations.Token);
        }

        private void Init(string token)
        {
            CommandManager.Sturtup();

            CommandManager.LoadCommands();

            //инициализация бота
            DiscordClient = new DiscordSocketClient();

            #region Обработчики событий

            DiscordClient.MessageReceived += DiscordClient_MessageReceived;
            DiscordClient.MessageUpdated += DiscordClient_MessageUpdated;
            //DiscordClient.Connected += () =>
            //{
            //    return Task.Factory.StartNew(() =>
            //    {
            //        if (!string.IsNullOrEmpty(HiPeople))
            //            foreach (var chanel in DiscordClient.DMChannels)
            //                chanel.SendMessageAsync(HiPeople);
            //    });
            //};

            #endregion Обработчики событий

            //подключение бота к серверу
            try
            {
                DiscordClient.LoginAsync(TokenType.Bot, token).Wait();
                DiscordClient.StartAsync().Wait();
            }
            catch (Exception ex)
            {
                throw new BotException($"Не удалось подключиться к дискорду.\n\tподробности: {ex.Message}", ex);
            }

            if (DiscordClient.CurrentUser != null)
                Mention = DiscordClient.CurrentUser.Mention;

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
        public DiscordSocketClient DiscordClient { get; private set; }

        /// <summary>
        /// Логер текущего бота.
        /// </summary>
        public ILoger Loger { get; set; }

        public string HiPeople { get; set; }
        
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

                if (text.ToLower().StartsWith("!bot"))
                    text = text.Replace("!bot", "").Trim();
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

        private Task DiscordClient_MessageUpdated(Discord.Cacheable<Discord.IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
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