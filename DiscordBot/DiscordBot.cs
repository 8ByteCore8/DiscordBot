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
                CommandManager = new DefaultControler();
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

            #endregion Обработчики событий

            //подключение бота к серверу
            try
            {
                discord.ConnectAsync().Wait();
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
        private DiscordClient DiscordClient { get; }

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
                ICommand command = CommandManager.Parse(text, CommandType.Console);
                command.ExecuteAsConsole(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //реакция на новое сообщение в дискорде
        private Task Discord_MessageCreated(MessageCreateEventArgs e)
        {
            return Task.Factory.StartNew(() =>
            {
                string text = e.Message.Content.Trim().ToLower();

                if (!text.StartsWith("!bot") || e.Message.Author.IsBot)
                    return;

                text = text.Replace("!bot ", "");

                try
                {
                    ICommand command = CommandManager.Parse(text, CommandType.Discord);

                    command.ExecuteAsBot(e);
                }
                catch (Exception ex)
                {
                    e.Message.RespondAsync(ex.Message);
                }
            });
        }
    }
}