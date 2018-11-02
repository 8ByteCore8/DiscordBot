﻿using DiscordBot.Common;
using DiscordBot.Common.Commands;
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
        /// Обект бота.
        /// </summary>
        private DiscordClient DiscordClient { get; }

        /// <summary>
        /// Логер текущего бота.
        /// </summary>
        public ILoger Loger { get; set; }

        public IControler Controler { get; set; }

        /// <summary>
        /// Создаёт новый обьект бота.
        /// </summary>
        /// <param name="token">Токен для подключения.</param>
        //public DiscordBot(string token)
        //{
        //    //проверка переданого токена
        //    if (token == null) throw new ArgumentNullException(nameof(token));

        //    //установка стандартного логера
        //    Loger = new DefaultLoger();

        //    //вызов инициализатора
        //    Init(token);
        //}

        /// <summary>
        /// Создаёт новый обьект бота.
        /// </summary>
        /// <param name="token">Токен для подключения.</param>
        /// <param name="loger">Пользовательский класс для логирования.</param>
        public DiscordBot(string token, ILoger loger = null, IControler controler = null)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (loger == null)
                Loger = new DefaultLoger();
            else
                Loger = loger;

            if (controler == null)
                Controler = new DefaultControler();
            else
                Controler = controler;

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
            try
            {
                discord.ConnectAsync().Wait();
            }
            catch (Exception ex)
            {
                throw new BotException($"не удалось подключиться к дискорду.\nподробности: {ex.Message}", ex);
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
                    ICommand command = Controler.Parse(text, CommandType.Discord);

                    command.ExecuteAsBot(e);
                }
                catch (Exception ex)
                {
                    e.Message.RespondAsync(ex.Message);
                }
            });
        }

        public void Execute(string text)
        {
            try
            {
                ICommand command = Controler.Parse(text, CommandType.Console);
                command.ExecuteAsConsole(DiscordClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
