using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DiscordBotCore.Common;
using DiscordBotCore.Common.Commands;

namespace DiscordBotCore
{
    /// <summary>
    /// Основной класс бота.
    /// </summary>
    public class DiscordBot : IDisposable
    {
        /// <summary>
        /// Обект бота.
        /// </summary>
        private DiscordClient DiscordClient { get; }

        /// <summary>
        /// Логер текущего бота.
        /// </summary>
        public ILoger Loger { get; set; }

        /// <summary>
        /// Создаёт новый обьект бота.
        /// </summary>
        /// <param name="token">Токен для подключения.</param>
        public DiscordBot(string token)
        {
            //проверка переданого токена
            if (token == null) throw new ArgumentNullException(nameof(token));

            //установка стандартного логера
            Loger = new DefaultLoger();

            //вызов инициализатора
            Init(token);
        }

        /// <summary>
        /// Создаёт новый обьект бота.
        /// </summary>
        /// <param name="token">Токен для подключения.</param>
        /// <param name="loger">Пользовательский класс для логирования.</param>
        public DiscordBot(string token, ILoger loger)
        {
            //проверка переданого токена
            if (token == null) throw new ArgumentNullException(nameof(token));

            //установка пользовательского логера
            Loger = loger ?? throw new ArgumentNullException(nameof(loger));

            //вызов инициализатора
            Init(token);
        }

        /// <summary>
        /// Инициализирует бота и подключает его к серверу.
        /// </summary>
        /// <param name="token">Токен для подключения.</param>
        private void Init(string token)
        {
            //инициализация бота
            DiscordClient discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            });

            #region Обработчики событий

            discord.MessageCreated += Discord_MessageCreated;

            #endregion

            //подключение бота к серверу
            discord.ConnectAsync().Wait();

            Loger.Log($"Бот успешно подключён.");
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~DiscordBot()
        {
            Dispose();
        }

        //реакция на новое сообщение в дискорде
        private Task Discord_MessageCreated(MessageCreateEventArgs e)
        {
            return Task.Factory.StartNew(() =>
            {
                string text = e.Message.Content.Trim().ToLower();

                if (!text.StartsWith("!bot")||e.Message.Author.IsBot)
                    return;

                text = text.Replace("!bot ","");

                IControler controler = new DefaultControler();
                try
                {
                    controler.Parse(text);
                }
                catch(Exception ex)
                {
                    e.Message.RespondAsync(ex.Message);
                }
            });
        }

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
    }
}
