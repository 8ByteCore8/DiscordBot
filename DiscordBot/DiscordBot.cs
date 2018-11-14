using DiscordBot.Attributes;
using DiscordBot.Common;
using DiscordBot.Exceptions;
using DSharpPlus;
using DSharpPlus.EventArgs;
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

            //инициализация бота
            DiscordClient discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            });

            #region Обработчики событий

            discord.MessageCreated += Discord_MessageCreated;
            discord.MessageUpdated += Discord_MessageUpdated;

            #endregion Обработчики событий

            //подключение бота к серверу
            try
            {
                discord.ConnectAsync().Wait();
                Mention = discord.CurrentUser.Mention;
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
        /// Логер текущего бота.
        /// </summary>
        public ILoger Loger { get; set; }

        /// <summary>
        /// Обект бота.
        /// </summary>
        public DiscordClient DiscordClient { get; }

        private string Mention { get; set; }

        /// <summary>
        /// Метод для коректного завершения работы бота и освобождения ресурсов.
        /// </summary>
        public void Dispose()
        {
            if (DiscordClient != null)
            {
                DiscordClient.DisconnectAsync();
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

        /// <summary>
        /// Реакция на новое сообщение.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Task Discord_MessageCreated(MessageCreateEventArgs e)
        {
            return Task.Factory.StartNew(() =>
            {
                string text = e.Message.Content.Trim();
                if (e.Message.Author.IsBot)
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
                        command.ExecuteAsBot(e.Message);
                }
                catch (Exception ex)
                {
                    e.Message.RespondAsync(ex.Message);
                }
            });
        }

        /// <summary>
        /// Реация на обновление сообщения.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Task Discord_MessageUpdated(MessageUpdateEventArgs e)
        {
            return Task.Factory.StartNew(() =>
            {
                if (DateTimeOffset.Now - e.Message.CreationTimestamp > new TimeSpan(1, 0, 0))
                    return;

                string text = e.Message.Content.Trim();
                if (e.Message.Author.IsBot)
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
                        command.ExecuteAsBot(e.Message);
                }
                catch (Exception ex)
                {
                    e.Message.RespondAsync(ex.Message);
                }
            });
        }
    }
}